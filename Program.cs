using System;
using System.Collections.Generic;

namespace HandBrakeInteropExample {
    using HandBrake.Interop;
    using HandBrake.Interop.Model;
    using HandBrake.Interop.Model.Encoding;

    class Program {
        private static HandBrakeInstance instance;
        private const string SourceFile = @"C:\Users\michael.de.keyser\Source\subtitle extract\unoptimized.mp4";

        static void Main(string[] args) {
            instance = new HandBrakeInstance();
            instance.Initialize(verbosity: 1);
            instance.ScanCompleted += instance_ScanCompleted;
            instance.StartScan(SourceFile, previewCount: 10);

            Console.ReadLine();
        }

        static EncodingProfile GetHandBrakeEncodingProfile() {
            return new EncodingProfile {
                PreferredExtension = OutputExtension.Mp4,
                IncludeChapterMarkers = false,
                LargeFile = false,
                Optimize = false,
                IPod5GSupport = false,
                Width = 0,
                Height = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                CroppingType = CroppingType.Automatic,
                Anamorphic = Anamorphic.Loose,
                UseDisplayWidth = false,
                DisplayWidth = 0,
                KeepDisplayAspect = false,
                PixelAspectX = 0,
                PixelAspectY = 2,
                Modulus = 2,
                Deinterlace = Deinterlace.Off,
                Denoise = Denoise.Off,
                Deblock = 0,
                Grayscale = false,
                VideoEncoder = "x264",                 
                VideoEncodeRateType = VideoEncodeRateType.ConstantQuality,
                Quality = 20,
                TargetSize = 0,
                VideoBitrate = 0,
                TwoPass = false,
                TurboFirstPass = false,
                Framerate = 0,
                ConstantFramerate = false,
                AudioEncodings = new List<AudioEncoding> {
                      new AudioEncoding {
                          InputNumber = 0,
                          Encoder = "av_aac",
                          EncodeRateType = AudioEncodeRateType.Bitrate,
                          Bitrate = 0,
                          Mixdown = "dpl2",
                          SampleRateRaw = 0,
                          Drc = 0
                      }
                  },
                AudioEncoderFallback = "av_aac"
            };
        }

        static void instance_ScanCompleted(object sender, EventArgs e) {
            Console.WriteLine("Scan complete");

            EncodingProfile profile = GetHandBrakeEncodingProfile();

            var job = new EncodeJob {
                EncodingProfile = profile,
                RangeType = VideoRangeType.All,
                Title = 1,
                SourcePath = SourceFile,

                ChosenAudioTracks = new List<int> { 1 },
                OutputPath = @"C:\Users\michael.de.keyser\Source\subtitle extract\hbOutput.mp4",
                Subtitles = new Subtitles {
                    SourceSubtitles = new List<SourceSubtitle>() {
                                new SourceSubtitle { TrackNumber = 1, BurnedIn = false, Default = false, Forced = false },
                                new SourceSubtitle { TrackNumber = 2, BurnedIn = false, Default = false, Forced = false }
                            },
                    SrtSubtitles = new List<SrtSubtitle>()
                }
            };

            instance.EncodeProgress += (o, args) => {
                Console.WriteLine(args.FractionComplete);
            };
            instance.EncodeCompleted += (o, args) => {
                Console.WriteLine("Encode completed.");
            };
            instance.StartEncode(job, 10);
        }
    }
}
