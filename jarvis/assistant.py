# -*- coding: utf-8 -*-
from fbchat import Client
import wolframalpha

from chatterbot import ChatBot
from chatterbot.trainers import ChatterBotCorpusTrainer
from tts import TTS
from stt import STT

wolfram_client = wolframalpha.Client('LXA9LJ-3835YR8529')

class Assistant(object):

    def __init__(self, use_speech):
        self.tts = TTS()

        self.use_speech = use_speech
        
        if self.use_speech:
            self.stt = STT()

        self.chatbot = ChatBot(
            'JARVIS',
            storage_adapter = 'chatterbot.storage.JsonFileStorageAdapter',
            filters = ['chatterbot.filters.RepetitiveResponseFilter'],
            logic_adapters=[
                {
                    'import_path': 'chatterbot.logic.BestMatch'
                }
            ]
        )
        
        self.chatbot.set_trainer(ChatterBotCorpusTrainer)
        self.chatbot.train("chatterbot.corpus.english.greetings")

    def search_wolfram(self, input):
        try:
            search_result = wolfram_client.query(input)
            self.say(next(search_result.results).text)
            return True
        except:
            return False

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

class Messenger(Client):

    responding = True
    
    def __init__(self, email, password):
	Client.__init__(self, email, password, logging_level = 50)

        self.chatbot = ChatBot(
            'FB JARVIS',
            storage_adapter = 'chatterbot.storage.JsonFileStorageAdapter',
            filters = ['chatterbot.filters.RepetitiveResponseFilter'],
            logic_adapters=[
                {
                    'import_path': 'chatterbot.logic.BestMatch'
                }
            ]
        )

        self.chatbot.set_trainer(ChatterBotCorpusTrainer)
        self.chatbot.train("chatterbot.corpus.english.greetings")
		
    def onMessage(self, author_id, message, thread_id, thread_type, **kwargs):
	self.markAsDelivered(author_id, thread_id)
	self.markAsRead(author_id)
	
	if author_id != self.uid:
            if self.responding and (thread_type != 'GROUP'):
                self.sendMessage('JARVIS: {}'.format(self.chatbot.get_response(message)),  thread_id, thread_type)
        else:
            if message == "JARVIS start responding":
                self.responding = True
            elif message == "JARVIS stop responding":
                self.responding = False
