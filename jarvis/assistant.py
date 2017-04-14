# -*- coding: utf-8 -*-

from chatterbot import ChatBot
from chatterbot.trainers import ChatterBotCorpusTrainer
from tts import TTS
from stt import STT

class Assistant(object):

    def __init__(self, use_speech):
        self.tts = TTS()

        self.use_speech = use_speech
        
        if self.use_speech:
            self.stt = STT()
        
        logic_adapters=[
            'chatterbot.logic.MathematicalEvaluation',
            'chatterbot.logic.TimeLogicAdapter'
        ]
        
        preprocessors=[
            'chatterbot.preprocessors.clean_whitespace'
        ]

        #chatbot = ChatBot('JARVIS',
        #                  storage_adapter='chatterbot.storage.MongoDatabaseAdapter')
        self.chatbot = ChatBot('JARVIS')
        
        self.chatbot.set_trainer(ChatterBotCorpusTrainer)
        self.chatbot.train("chatterbot.corpus.english.greetings")

        
    def say(self, message):
        self.tts.say(message)

    def respond(self, message):
        self.say(self.chatbot.get_response(message))

    def get_input(self):
        if (self.use_speech):
            input = self.stt.listen()
            print 'User: {}'.format(input)
        else:
            input = raw_input('User: ')

        return input

    
