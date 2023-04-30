using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Resources;
using speech2;
using System.Reflection;

class Program
{
    // This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
    static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
    static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

    static async Task Main(string[] args)
    {
        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);

        // The language of the voice that speaks.
        speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";

        // SSML to synthesize.
      //  string ssml = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'><voice name='en-US-JennyNeural'><prosody rate='1.0'>Hello, world!</prosody></voice></speak>";
        var assembly = Assembly.GetExecutingAssembly();
        var resourceManager = new ResourceManager("speech2.Resource1", assembly);
        var file = resourceManager.GetString("ssml");


        using (var synthesizer = new SpeechSynthesizer(speechConfig))
        {
            using (var result = await synthesizer.SpeakSsmlAsync(file))
            {
                if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                {
                    using (var audioDataStream = AudioDataStream.FromResult(result))
                    {
                        // Save audio data to a file
                        await audioDataStream.SaveToWaveFileAsync("output2.wav");

                        Console.WriteLine($"Audio file saved to {Path.GetFullPath("output.wav")}");
                    }
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                    }
                }
            }
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
