#!/bin/zsh
cd "${0:A:h}" || exit 1
python3 tools/cinder_four_player.py start --companion
