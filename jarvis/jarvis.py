# -*- coding: utf-8 -*-

from chatterbot import ChatBot
from chatterbot.trainers import ChatterBotCorpusTrainer
from tts import TTS
from stt import STT

import nltk

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

input = ''

tts.say("I have been fully loaded")

use_speech = False

if use_speech:
    stt = STT()

while (input != 'quit'):
    try:
        if (use_speech):
            input = stt.listen()
            print 'User: {}'.format(input)
        else:
            input = raw_input('User: ')

        if not input == '':
            words = nltk.word_tokenize(input)
            print nltk.pos_tag(words)
            tts.say(chatbot.get_response(input))
    except Exception as e:
        print e
        break

