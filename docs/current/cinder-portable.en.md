# NO RETURNS — running on another PC

[한국어](cinder-portable.ko.md)

2026-09-18 · CINDER-DEMO-01 · Internal Windows 64-bit test package

1. Copy the entire package folder to the other Windows PC. Extract the entire archive first if transferred as a ZIP.
2. Run `PLAY.cmd` or `NoReturns-CinderDemo.exe`. Unity installation is not required.
3. Create a room for solo play. For co-op, everyone must use the same build and join before departure.
4. On the same router/LAN, the host creates a room and peers enter the host PC's IPv4 address before joining. On the host, use `ipconfig` to find the IPv4 address of the active network adapter.
5. If Windows Firewall asks for network access, allow it on the trusted private network you are using. The connection uses TCP 27841. Do not disable the entire firewall.

`127.0.0.1` works only on the same PC. Automatic connections between different homes/over the public internet and Steam invitations are currently unsupported. Final LAN validation on another physical PC remains outstanding.

Keep `NoReturns-CinderDemo_Data`, `MonoBleedingEdge`, `D3D12`, DLLs and the other supplied files beside the executable. Sending only the EXE will not work. `runtime-manifest.json` lists SHA-256 comparisons between original and copied runtime files. Development backups, the project and personal progression are excluded. Progress is saved separately for the host account on the playing PC.

WASD movement, mouse look, E interaction/hold to rescue a teammate, Q set down, Shift quiet walking, Space empty-handed jump, C call, left click baton, Esc menu. Switch languages in the menu.

Press E aboard to select a contract/depart → pick up cargo with E and carry it to BAY 04 → place it on the marked floor with Q → collect the printed receipt at the terminal with E → bring everyone aboard and press E to return/settle. The first standard contract pays 420CR.

This is a primitive-map demo. Human enjoyment, compatibility with all PCs and Steam release are not validated.


**Version compatibility:** the September 18 portable package uses protocol 11. This dense facility build uses protocol 12 and cannot connect to it. Copy the complete new build to both computers before testing.
