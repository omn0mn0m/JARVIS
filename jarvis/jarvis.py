# -*- coding: utf-8 -*-

from chatterbot import ChatBot
from chatterbot.trainers import ChatterBotCorpusTrainer
from tts import TTS

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

while (input != 'quit'):
    try:
        input = raw_input('User: ')
        tts.say(chatbot.get_response(input))
    except:
        break

