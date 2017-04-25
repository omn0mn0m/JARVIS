import nltk
import filemanager

from assistant import Assistant

use_speech = False
nlp_debug = True

jarvis = Assistant(use_speech)

jarvis.say('I have been fully loaded')

input = ''

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
                jarvis.say(filemanager.try_open_executable(words, tagged))
            else:
                jarvis.respond(input)
    except Exception as e:
        print e
        break

