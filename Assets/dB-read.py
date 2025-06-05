# First we have to install sounddevice and numpy library on the computer
# in shell:
# 			pip install sounddevice numpy
# NOTE: you might have to use pip3 or pip3.1x depending on the pip installation
# NOTE: you might have to run this program as sudo, as it uses a microphone

# The following program prints the mean relative dB value for a given duration

import sounddevice as sd
import numpy as np

def get_microphone_level(duration=0.1, sample_rate=44100):
	# Capture a short audio clip from the microphone and return its dB level.
	recording = sd.rec(int(duration * sample_rate), samplerate=sample_rate, channels=1, dtype='float32')
	sd.wait()  # Wait for recording to finish
	
	rms = np.sqrt(np.mean(recording**2))  # Root Mean Square
	db = 20 * np.log10(rms) if rms > 0 else -np.inf
	return db

print(f'{get_microphone_level():.2f}')