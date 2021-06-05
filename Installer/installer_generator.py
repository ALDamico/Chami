import os
import pathlib
import re

lines = []

NSIS_SCRIPT_PATH = pathlib.Path(__file__).parent
NSIS_SCRIPT_PATH = pathlib.Path(str(NSIS_SCRIPT_PATH) + "/chami-installer.source.nsi")
NSIS_OUT_SCRIPT_PATH = pathlib.Path(__file__).parent
NSIS_OUT_SCRIPT_PATH = pathlib.Path(str(NSIS_OUT_SCRIPT_PATH) + "/chami-installer.target.nsi")
CHAMI_BIN_PATH = NSIS_SCRIPT_PATH.parent.parent

CHAMI_BIN_PATH = pathlib.Path(str(CHAMI_BIN_PATH) + "/ChamiUI/bin/Release/net5.0-windows")

with open(str(NSIS_SCRIPT_PATH), 'r') as input_file:
    lines = input_file.readlines()


with open(str(NSIS_OUT_SCRIPT_PATH), 'w') as output_file:
    for line in lines:
        if re.match(r"\s*\[\[FILE\sTO\sINSTALL\]\]", line):
            output_file.write(f"File /r \"{str(CHAMI_BIN_PATH)}\\*\"")
            continue 
        output_file.write(line)

#for full_path, _, file_name in os.walk(str(CHAMI_BIN_PATH)):
    

exit(0)