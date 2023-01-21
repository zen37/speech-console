using System;
using System.ComponentModel;
using System.IO;
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



    static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
    {
        switch (speechSynthesisResult.Reason)
        {
            case ResultReason.SynthesizingAudioCompleted:
                Console.WriteLine($"Speech synthesized for text: [{text}]");
                break;
            case ResultReason.Canceled:
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }
                break;
            default:
                break;
        }
    }

    async static Task Main(string[] args)
    {
        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);

        // The language of the voice that speaks.
        speechConfig.SpeechSynthesisVoiceName = "en-US-NancyNeural";

        using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
        {
            // Get text from the console and synthesize to the default speaker.
            //   Console.WriteLine("Enter some text that you want to speak >");
            //   string text = Console.ReadLine();

            //var file = speech2.Resource1.text_txt;

            // var file = Properties.Resource1.text_txt;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceManager = new ResourceManager("speech2.Resource1", assembly);
            var file = resourceManager.GetString("text");
            //   Console.WriteLine(resource);


            // string fileName = @"..\text.txt";
            // string text = File.ReadAllText(file);

            var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(file);
           // var speechSynthesisResult = await speechSynthesizer.SpeakSsmlAsync(file);

           OutputSpeechSynthesisResult(speechSynthesisResult, file);
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}