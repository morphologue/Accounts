#!/usr/bin/env bash
# Convenience installer for Linux/macOS
set -e

INSTALLLIBDIR="$HOME/.local/share/morphologue/accounts/lib"
INSTALLBINLN="$HOME/bin/accounts"
TARGETSDIR="bin/pub/targets"
RID=${1:-osx.12-arm64}

rm -rf "$INSTALLLIBDIR" "$INSTALLBINLN"
mkdir -p "$INSTALLLIBDIR" "$(dirname "$INSTALLBINLN")"
cp -r "$TARGETSDIR/$RID/" "$INSTALLLIBDIR"
ln -s "$INSTALLLIBDIR/Morphologue.Accounts.Presentation.ConsoleApp" "$INSTALLBINLN"
