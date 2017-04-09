# -*- coding: utf-8 -*-

from chatterbot import ChatBot
from chatterbot.trainers import ChatterBotCorpusTrainer
from tts import TTS

import speech_recognition as sr

tts = TTS()

logic_adapters=[
        'chatterbot.logic.MathematicalEvaluation',
        'chatterbot.logic.TimeLogicAdapter'
]

preprocessors=[
    'chatterbot.preprocessors.clean_whitespace'
]

#chatbot = ChatBot('JARVIS',
#                  storage_adapter='chatterbot.storage.MongoDatabaseAdapter')
chatbot = ChatBot('JARVIS')

chatbot.set_trainer(ChatterBotCorpusTrainer)
chatbot.train("chatterbot.corpus.english.greetings")

recogniser = sr.Recognizer()
mic = sr.Microphone()

with mic  as source:
    print "A moment of silence please..."
    recogniser.adjust_for_ambient_noise(source)
    
input = ''

tts.say("I have been fully loaded")

use_speech = True

while (input != 'quit'):
    try:
        if (use_speech):
            print "Say something!"

            with mic as source:
                audio = recogniser.listen(source)

            try:
                input = recogniser.recognize_google(audio)
                print 'User: {}'.format(input)
                tts.say(chatbot.get_response(input))
            except sr.UnknownValueError:
                print "I didn't quite get that..."
            except sr.RequestError as e:
                print "Google Speech isn't working..."
        else:
            input = raw_input('User: ')
            tts.say(chatbot.get_response(input))
    except:
        break

