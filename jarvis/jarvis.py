import nltk
import filemanager
import multiprocessing
import os
import ConfigParser

from assistant import Assistant, Messenger

resources_dir = 'resources\\'

login_creds = ConfigParser.SafeConfigParser()

if os.path.isfile(resources_dir + 'login_creds.cfg'):
    login_creds.read(resources_dir + 'login_creds.cfg')
else:
    print "No logins... creating now"
    new_login_creds = open(resources_dir + 'login_creds.cfg', 'w')
    login_creds.write(new_login_creds)
    new_login_creds.close()
    

def fb_worker(email, password):
    messenger = Messenger(email, password)
    messenger.listen()
    return

if __name__ == '__main__':
    use_speech = False
    nlp_debug = True

    jarvis = Assistant(use_speech)
    
    jarvis.say('I have been fully loaded')
    
    input = ''

    while (input != 'Goodbye JARVIS'):
        try:
            input = jarvis.get_input()
            
            if not input == '':
                words = nltk.word_tokenize(input)
                tagged = nltk.pos_tag(words)
                
                verbs = []
                proper_nouns = []
                
                for word in tagged:
                    if 'VB' in word[1]:
                        verbs.append(word[0].lower())
                    elif word[1] == 'NNP':
                        proper_nouns.append(word[0].lower())
                        
                if nlp_debug:
                    print 'Tags: {}'.format(tagged)
                    print 'Verbs: {}'.format(verbs)
                    
                if "open" in verbs:
                    jarvis.say(filemanager.try_open_executable(words, tagged))
                elif "respond" in verbs:
                    if "facebook" in proper_nouns:
                        if not login_creds.has_section('Facebook'):
                            login_creds.add_section('Facebook')
                            login_creds.set('Facebook', 'email', raw_input('Enter your FB email: '))
                            login_creds.set('Facebook', 'password', raw_input('Enter your FB password: '))

                        with open(resources_dir + 'login_creds.cfg', 'wb') as configfile:
                            login_creds.write(configfile)

                        fb_process = multiprocessing.Process(target = fb_worker, args = (login_creds.get('Facebook', 'email'), login_creds.get('Facebook', 'password')))
                        fb_process.daemon = True
                        fb_process.start()
                else:
                    jarvis.respond(input)
        except Exception as e:
            print e
            fb_process.terminate()
            fb_process.join()
            break
