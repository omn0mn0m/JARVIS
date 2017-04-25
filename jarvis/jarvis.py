import nltk
import actions
import ConfigParser
from assistant import Assistant
from operator import itemgetter

use_speech = False
nlp_debug = True

resources_dir = 'resources\\'

jarvis = Assistant(use_speech)

executable_paths = ConfigParser.SafeConfigParser()

if os.path.isfile(resources_dir + 'executable_paths.cfg'):
    executable_paths.read(resources_dir + 'executable_paths.cfg')
else:
    print "No executables list... creating now"
    new_executable_paths = open(resources_dir + 'executable_paths.cfg', 'w')
    executable_paths.write(new_executable_paths)
    new_executable_paths.close()

jarvis.say('I have been fully loaded')

input = ''

def collect(tuple_list, index):
    return map(itemgetter(index), tuple_list)

while (input != 'quit'):
    try:
        input = jarvis.get_input()

        if not input == '':
            words = nltk.word_tokenize(input)
            tagged = nltk.pos_tag(words)
            
            verbs = []
            
            for word in tagged:
                if 'VB' in word[1]:
                    verbs.append(word[0].lower())

            if nlp_debug:
                print 'Tags: {}'.format(tagged)
                print 'Verbs: {}'.format(verbs)

            if "open" in verbs:
                executable = words[collect(tagged, 0).index('open') + 1]

                if executable_paths.has_option('DEFAULT', executable):
                    filepath = executable_paths.get('DEFAULT', executable)
                else:
                    filepath = actions.find_file(executable, "C:\\Program Files\\")

                    if filepath == '':
                        filepath = actions.find_file(executable, "C:\\Program Files (x86)\\")
                    
                print filepath
                
                if filepath != '':
                    actions.run_file(filepath)

                    try:
                        executable_paths.set('DEFAULT', executable, filepath)
                    except NoSectionError:
                        executable_paths.add_section('DEFAULT')
                        executable_paths.set('DEFAULT', executable, filepath)

                    with open(resources_dir + 'executable_paths.cfg', 'wb') as configfile:
                        executable_paths.write(configfile)

                    executable_paths.read(resources_dir + 'executable_paths.cfg')
                    
                    jarvis.say("Opening your program.")
                else:
                    jarvis.say("I couldn't find your program...")
            else:
                jarvis.respond(input)
    except Exception as e:
        print e
        break

