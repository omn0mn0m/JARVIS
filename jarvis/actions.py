import os
import subprocess
from difflib import SequenceMatcher

tolerance = 0.7

def find_file(name, path):
    for root, dirs, files in os.walk(path):
        for filename in files:
            fileTok = filename.lower().split('.')
            similarity = SequenceMatcher(None, name, fileTok[0]).ratio()
            print '{}: {}'.format(similarity, fileTok)
            
            if (similarity >= tolerance):
                print 'Found'
                return os.path.join(root, filename)
            
    return ''

def run_file(path):
    subprocess.Popen(path)
