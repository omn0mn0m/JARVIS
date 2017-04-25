import os
import subprocess
import ConfigParser

from difflib import SequenceMatcher
from operator import itemgetter

resources_dir = 'resources\\'
tolerance = 0.88

executable_paths = ConfigParser.SafeConfigParser()

if os.path.isfile(resources_dir + 'executable_paths.cfg'):
    executable_paths.read(resources_dir + 'executable_paths.cfg')
else:
    print "No executables list... creating now"
    new_executable_paths = open(resources_dir + 'executable_paths.cfg', 'w')
    executable_paths.write(new_executable_paths)
    new_executable_paths.close()

def collect(tuple_list, index):
    return map(itemgetter(index), tuple_list)
	
def find_file(name, path):
    for root, dirs, files in os.walk(path):
        for filename in files:
            fileTok = filename.lower().split('.')
            similarity = SequenceMatcher(None, name, fileTok[0]).ratio()
            
            if (similarity >= tolerance):
				filepath = os.path.join(root, filename)
				print 'Found: {} with {}% similarity'.format(filepath, similarity)
				return filepath
    return ''

def run_file(path):
    subprocess.Popen(path)

def try_open_executable(words, tagged):
	executable = words[collect(tagged, 0).index('open') + 1]

	if executable_paths.has_option('DEFAULT', executable):
		filepath = executable_paths.get('DEFAULT', executable)
	else:
		filepath = find_file(executable, "C:\\Program Files\\")

		if filepath == '':
			filepath = find_file(executable, "C:\\Program Files (x86)\\")
	
	if filepath != '':
		run_file(filepath)

		try:
			executable_paths.set('DEFAULT', executable, filepath)
		except NoSectionError:
			executable_paths.add_section('DEFAULT')
			executable_paths.set('DEFAULT', executable, filepath)

		with open(resources_dir + 'executable_paths.cfg', 'wb') as configfile:
			executable_paths.write(configfile)

		executable_paths.read(resources_dir + 'executable_paths.cfg')
		
		return "Opening your program."
	else:
		return "I couldn't find your program..."