import os
import subprocess

def find_file(name, path):
    for root, dirs, files in os.walk(path):
        if name in files:
            return os.path.join(root, name)

    return ''

def run_file(path):
    subprocess.Popen(path)
