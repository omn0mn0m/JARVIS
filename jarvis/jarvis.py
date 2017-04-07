from tts import TTS

tts = TTS()

input = ''

tts.say('I have been fully loaded')

while (input != 'quit'):
    input = raw_input('User: ')
    tts.say(input)

