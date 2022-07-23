#!/usr/bin/env bash
set -e

VERSION="1.0.0"
TARGETSDIR="bin/pub/targets"
FINALBASENAME="accounts"

rm -rf "$TARGETSDIR"
for RID in osx.12-x64 osx.12-arm64 linux-x64 linux-arm linux-arm64 win10-x64 win10-arm64; do
  TARGETDIR="$TARGETSDIR/$RID"
  mkdir -p "$TARGETDIR"
  dotnet publish -c Release -o "$TARGETDIR" -r $RID --self-contained true
done

cd "$TARGETSDIR"
zip -r "../Release-$VERSION.zip" .
