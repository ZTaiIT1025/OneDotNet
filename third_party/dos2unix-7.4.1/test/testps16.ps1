# This script demonstrates how to use Unicode file names
# in a PowerShell script.
# This script is in UTF-16 encoding.

$env:DOS2UNIX_DISPLAY_ENC = "unicode"

dos2unix -i uni_el_αρχείο.txt uni_zh_文件.txt


echo "test select-string:"

# select-string requires a BOM.
$env:DOS2UNIX_DISPLAY_ENC = "unicodebom"
dos2unix -i uni* | select-string -encoding unicode -pattern αρχ

$env:DOS2UNIX_DISPLAY_ENC = ""
