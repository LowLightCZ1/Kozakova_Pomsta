#!/usr/bin/env bash
# ============================================================================
# Kozakova_Pomsta — batched push
# Run with GIT BASH, from the project root (where .gitignore/.gitattributes
# from reinit_repo.sh already exist). This wipes any current .git (your old
# broken history is already safe in .git_broken_backup from before, so this
# is safe to redo) and re-commits + pushes everything in ~500MB chunks, so
# no single push exceeds GitHub's 2GiB pack limit.
# ============================================================================
set -e

if [ ! -f ".gitignore" ] || [ ! -f ".gitattributes" ]; then
  echo "ERROR: .gitignore/.gitattributes not found. Run this from the project"
  echo "root, after reinit_repo.sh has already been run once."
  exit 1
fi

TARGET_BYTES=$((500*1024*1024))   # ~500MB per push, well under the 2GiB cap
REMOTE_URL="git@github.com:LowLightCZ1/Kozakova_Pomsta.git"

echo "############################################################"
echo "STEP 0: Fresh git init (old broken history stays in .git_broken_backup)"
echo "############################################################"
if [ -d ".git" ]; then
  rm -rf .git
fi
git init -b main
git remote add origin "$REMOTE_URL"

echo ""
echo "############################################################"
echo "STEP 1: Building list of files to commit, respecting .gitignore"
echo "############################################################"

TMP_LIST=$(mktemp)

collect() {
  local dir="$1"
  while IFS= read -r -d '' entry; do
    name="$(basename "$entry")"
    [ "$name" = ".git" ] && continue
    [ "$name" = ".git_broken_backup" ] && continue
    # skip anything .gitignore excludes
    if git check-ignore -q "$entry"; then
      continue
    fi
    if [ -d "$entry" ]; then
      size=$(du -sb "$entry" 2>/dev/null | cut -f1)
      if [ "$size" -gt "$TARGET_BYTES" ]; then
        collect "$entry"
      else
        printf '%s\t%s\n' "$size" "$entry" >> "$TMP_LIST"
      fi
    else
      size=$(stat -c%s "$entry" 2>/dev/null || wc -c < "$entry")
      printf '%s\t%s\n' "$size" "$entry" >> "$TMP_LIST"
    fi
  done < <(find "$dir" -mindepth 1 -maxdepth 1 -print0)
}

collect "."
sort -t $'\t' -k1,1 -rn "$TMP_LIST" -o "$TMP_LIST"
CHUNK_COUNT=$(wc -l < "$TMP_LIST")
echo "Found $CHUNK_COUNT items to distribute across batches."

echo ""
echo "############################################################"
echo "STEP 2: Packing into ~500MB batches (first-fit-decreasing)"
echo "############################################################"

declare -a bin_sizes
declare -a bin_files
bin_count=0

while IFS=$'\t' read -r size path; do
  [ -z "$path" ] && continue
  placed=0
  for ((i=0; i<bin_count; i++)); do
    if (( bin_sizes[i] + size <= TARGET_BYTES )); then
      bin_sizes[i]=$((bin_sizes[i] + size))
      bin_files[i]="${bin_files[i]}"$'\n'"$path"
      placed=1
      break
    fi
  done
  if [ "$placed" -eq 0 ]; then
    bin_sizes[bin_count]=$size
    bin_files[bin_count]="$path"
    bin_count=$((bin_count+1))
  fi
done < "$TMP_LIST"

echo "Packed into $bin_count batches."
rm -f "$TMP_LIST"

echo ""
echo "############################################################"
echo "STEP 3: Committing and pushing each batch"
echo "############################################################"

for ((i=0; i<bin_count; i++)); do
  n=$((i+1))
  mb=$(( bin_sizes[i] / 1024 / 1024 ))
  echo ""
  echo "==== Batch $n / $bin_count  (~${mb} MB) ===="
  while IFS= read -r p; do
    [ -z "$p" ] && continue
    git add -- "$p"
  done <<< "${bin_files[i]}"

  if git diff --cached --quiet; then
    echo "(nothing staged, skipping commit/push)"
    continue
  fi

  git commit -q -m "Add project files - batch $n/$bin_count"

  if [ "$n" -eq 1 ]; then
    echo "Pushing batch $n (force, establishes new history on remote)..."
    git push --force -u origin main
  else
    echo "Pushing batch $n..."
    git push origin main
  fi
done

echo ""
echo "############################################################"
echo "ALL BATCHES PUSHED."
echo "############################################################"
echo "Your collaborator should now delete their old clone entirely and"
echo "re-clone: git clone $REMOTE_URL"
