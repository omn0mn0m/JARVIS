# -*- coding: utf-8 -*-
import fbchat
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
        search_result = wolfram_client.query(input)
        self.say(next(search_result.results).text)

        
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

class Messenger(fbchat.Client):

    responding = True
    
    def __init__(self, email, password, debug = False, user_agent = None):
	fbchat.Client.__init__(self, email, password, debug, user_agent)

        self.chatbot = ChatBot(
            'FB JARVIS',
            storage_adapter = 'chatterbot.storage.JsonFileStorageAdapter',
            filters = ['chatterbot.filters.RepetitiveResponseFilter'],
            logic_adapters=[
                {
                    'import_path': 'chatterbot.logic.MathematicalEvaluation'
                },
                {
                    'import_path': 'chatterbot.logic.TimeLogicAdapter'
                },
                {
                    'import_path': 'chatterbot.logic.BestMatch'
                }
            ]
        )

        self.chatbot.set_trainer(ChatterBotCorpusTrainer)
        self.chatbot.train("chatterbot.corpus.english.greetings")
		
    def on_message_new(self, mid, author_id, message, metadata, recipient_id, thread_type):
	self.markAsDelivered(recipient_id, mid)
	self.markAsRead(recipient_id)
	
	if str(author_id) != str(self.uid):
            if self.responding and (thread_type != 'group'):
                self.send(recipient_id, 'JARVIS: {}'.format(self.chatbot.get_response(message)), thread_type)
        else:
            if message == "JARVIS start responding":
                self.responding = True
            elif message == "JARVIS stop responding":
                self.responding = False
