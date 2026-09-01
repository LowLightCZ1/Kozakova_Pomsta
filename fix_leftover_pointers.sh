#!/usr/bin/env bash
# ============================================================================
# Restores the last 2 files that were still broken LFS pointers even in the
# fresh commit (they were already broken on disk before we started - this
# pulls the real content out of .git_broken_backup, the history saved by
# reinit_repo.sh).
# Run with GIT BASH, from the project root.
# ============================================================================
set -e

if [ ! -d ".git_broken_backup" ]; then
  echo "ERROR: .git_broken_backup not found. Run this from the project root."
  exit 1
fi

F1="SourceMaterials_Blend/materials/wood_f56e42ae-559b-47ca-add0-f6a56a1ef1d7/wood_2K_dea41e60-e086-43ca-b852-7ca4bf825471.blend"
F2="SourceMaterials_Blend/materials/wood-fine_2f5fa5d6-8e8b-44b6-83a3-5693113bceb0/wood-fine_2K_1b230e28-ee0b-4d49-b0b2-d2b8d28b4616.blend"

echo "Extracting real content from backup history..."
git --git-dir=.git_broken_backup cat-file -p f7604c7f5c849ccf6cc17c8e4eaebdf842d44046 > "$F1"
git --git-dir=.git_broken_backup cat-file -p 651e9e11018c6ed43702dfe5de6643563e80442d > "$F2"

echo "Verifying sizes..."
wc -c "$F1" "$F2"

echo "Committing and pushing..."
git add "$F1" "$F2"
git commit -m "Restore 2 wood material .blend files that were still LFS pointer stubs"
git push origin main

echo "Done."
