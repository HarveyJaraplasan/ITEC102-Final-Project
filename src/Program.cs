using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text;
using static System.Console;

namespace Hardis_Proj
{
    internal class Program
    {
        // Local variables
        public static bool withsounds;

        // Current system information
        private static double cpuCores;
        private static double cpuThreads;
        private static double cpuSpeedGHz;
        private static double ramGB;
        private static double ramSpeedMHz;
        private static double totalStorageGB;
        private static double totalLogicalStorageGB;
        private static double totalFreeStorageGB;
        private static readonly StringBuilder scanDetails = new StringBuilder();

        // MAIN METH
        static void Main(string[] args)
        {
            Title = "Hardware Recognition and Detection Identification System";
            CursorVisible = false;

            ForegroundColor = ConsoleColor.Green;
            Write("Do you wish to use sounds? Y / N: ");
            ForegroundColor = ConsoleColor.Magenta;

            string soundChoice = ReadLine();
            withsounds = soundChoice != null &&
                         soundChoice.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase);

            ForegroundColor = ConsoleColor.Green;

            if (withsounds)
            {
                Beep();
            }

            WriteLine("\nBy: Harvey. Dwane. Aldrin");
            WriteLine("Welcome to the HARDIS Software");
            WriteLine("It will check your systems' components informations.\r\n");
            Write("Do you wish to proceed? Y / N: ");
            ForegroundColor = ConsoleColor.Magenta;

            string proceed = ReadLine();

            if (proceed != null && proceed.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                if (withsounds)
                {
                    Beep();
                }

                ForegroundColor = ConsoleColor.White;
                WriteLine("\nScanning system components...\n");

                CheckSpecs();
                ShowSystemOverview();
            }

            // This is a secret :)
            else if (proceed != null && proceed.Trim().Equals("$#", StringComparison.OrdinalIgnoreCase))
            {
                string imagePath = "https://media1.tenor.com/m/SIpmtvnEsDIAAAAd/rotating-chips.gif";

                Process.Start(new ProcessStartInfo
                {
                    FileName = imagePath,
                    UseShellExecute = true // Required in modern .NET (.NET Core / .NET 5+)
                });
            }

            else
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("\nOperation cancelled.");
            }

            ForegroundColor = ConsoleColor.Green;
            WriteLine("\nPress any key to exit.");
            ReadKey();
            ResetColor();
        }

        // CPU METH
        static void GetCpuInfo()
        {
            if (withsounds)
            {
                Beep();
            }

            ForegroundColor = ConsoleColor.Green;
            WriteLine("\n================ CPU ================");
            ForegroundColor = ConsoleColor.Blue;
            scanDetails.AppendLine("CPU");

            using (ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string name = Convert.ToString(item["Name"]);
                    double cores = Convert.ToDouble(item["NumberOfCores"] ?? 0);
                    double threads = Convert.ToDouble(item["NumberOfLogicalProcessors"] ?? 0);
                    double speedMHz = Convert.ToDouble(item["MaxClockSpeed"] ?? 0);
                    double currentMHz = Convert.ToDouble(item["CurrentClockSpeed"] ?? 0);

                    cpuCores += cores;
                    cpuThreads += threads;
                    cpuSpeedGHz = Math.Max(cpuSpeedGHz, speedMHz / 1000.0);

                    string cpuInfo =
                        "Processor       : " + name + Environment.NewLine +
                        "Cores           : " + cores + Environment.NewLine +
                        "Logical Threads : " + threads + Environment.NewLine +
                        "Maximum Speed   : " + (speedMHz / 1000.0).ToString("0.00") + " GHz" + Environment.NewLine +
                        "Current Speed   : " + (currentMHz / 1000.0).ToString("0.00") + " GHz";

                    WriteLine(cpuInfo);
                    scanDetails.AppendLine(cpuInfo);
                    scanDetails.AppendLine("-------------------------------------");
                }
            }
        }

        // RAM METH
        static void GetRamInfo()
        {
            if (withsounds)
            {
                Beep();
            }

            ForegroundColor = ConsoleColor.Green;
            WriteLine("\n================ RAM ================");
            ForegroundColor = ConsoleColor.Blue;
            scanDetails.AppendLine();
            scanDetails.AppendLine("RAM MODULES");

            using (ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory"))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string manufacturer = Convert.ToString(item["Manufacturer"]);
                    string partNumber = Convert.ToString(item["PartNumber"]);
                    double capacityBytes = Convert.ToDouble(item["Capacity"] ?? 0);
                    double speed = Convert.ToDouble(item["Speed"] ?? 0);

                    ramGB += capacityBytes / 1024.0 / 1024.0 / 1024.0;
                    ramSpeedMHz = Math.Max(ramSpeedMHz, speed);

                    string ramInfo =
                        "Manufacturer : " + manufacturer + Environment.NewLine +
                        "Part Number  : " + partNumber + Environment.NewLine +
                        "Capacity     : " + (capacityBytes / 1024.0 / 1024.0 / 1024.0).ToString("0.00") + " GB" + Environment.NewLine +
                        "Speed        : " + speed + " MHz";

                    WriteLine(ramInfo);
                    scanDetails.AppendLine(ramInfo);
                    scanDetails.AppendLine("-------------------------------------");
                }
            }

            WriteLine("Total RAM   : " + ramGB.ToString("0.00") + " GB");
            WriteLine("RAM Speed   : " + ramSpeedMHz + " MHz");
            scanDetails.AppendLine("Total RAM: " + ramGB.ToString("0.00") + " GB");
            scanDetails.AppendLine("RAM Speed: " + ramSpeedMHz + " MHz");
        }

        // STORAGE METH
        static void GetStorageInfo()
        {
            if (withsounds)
            {
                Beep();
            }

            ForegroundColor = ConsoleColor.Green;
            WriteLine("\n================ STORAGE ================");
            ForegroundColor = ConsoleColor.Blue;
            scanDetails.AppendLine();
            scanDetails.AppendLine("PHYSICAL DISKS");

            using (ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    if (withsounds)
                    {
                        Beep();
                    }

                    string model = Convert.ToString(item["Model"]);
                    string name = Convert.ToString(item["Name"]);
                    string interfaceType = Convert.ToString(item["InterfaceType"]);
                    double sizeBytes = Convert.ToDouble(item["Size"] ?? 0);
                    double sizeGB = sizeBytes / 1024.0 / 1024.0 / 1024.0;

                    totalStorageGB += sizeGB;

                    string diskInfo =
                        "Drive       : " + name + Environment.NewLine +
                        "Model       : " + model + Environment.NewLine +
                        "Interface   : " + interfaceType + Environment.NewLine +
                        "Capacity    : " + sizeGB.ToString("0.00") + " GB";

                    WriteLine(diskInfo);
                    scanDetails.AppendLine(diskInfo);
                    scanDetails.AppendLine("-----------------------------------------");
                }
            }

            // Logical drive volumes and free space
            WriteLine("\nLogical Volumes:");
            scanDetails.AppendLine();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (withsounds)
                {
                    Beep();
                }

                if (drive.IsReady)
                {
                    double volumeTotalGB = drive.TotalSize / 1024.0 / 1024.0 / 1024.0;
                    double volumeFreeGB = drive.AvailableFreeSpace / 1024.0 / 1024.0 / 1024.0;

                    totalLogicalStorageGB += volumeTotalGB;
                    totalFreeStorageGB += volumeFreeGB;

                    string volumeInfo = drive.Name +
                        " | Total: " + volumeTotalGB.ToString("0.00") + " GB" +
                        " | Free: " + volumeFreeGB.ToString("0.00") + " GB";

                    WriteLine(volumeInfo);
                    scanDetails.AppendLine(volumeInfo);
                }
            }

            WriteLine("\nTotal Physical Storage: " + totalStorageGB.ToString("0.00") + " GB");
            WriteLine("Total Logical Storage : " + totalLogicalStorageGB.ToString("0.00") + " GB");
            WriteLine("Total Free Storage    : " + totalFreeStorageGB.ToString("0.00") + " GB");

            scanDetails.AppendLine();
            scanDetails.AppendLine("Total Physical Storage: " + totalStorageGB.ToString("0.00") + " GB");
            scanDetails.AppendLine("Total Logical Storage : " + totalLogicalStorageGB.ToString("0.00") + " GB");
            scanDetails.AppendLine("Total Free Storage    : " + totalFreeStorageGB.ToString("0.00") + " GB");

            // Live disk read/write counters (Windows performance counters)
            WriteLine("\nLive Disk Activity (bytes/sec):");
            try
            {
                if (withsounds)
                {
                    Beep();
                }

                using (PerformanceCounter readCounter =
                       new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total"))
                using (PerformanceCounter writeCounter =
                       new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total"))
                {
                    readCounter.NextValue();
                    writeCounter.NextValue();
                    System.Threading.Thread.Sleep(1000);

                    double readBytes = readCounter.NextValue();
                    double writeBytes = writeCounter.NextValue();

                    string activityInfo =
                        "Read  : " + FormatBytesPerSecond(readBytes) + Environment.NewLine +
                        "Write : " + FormatBytesPerSecond(writeBytes);

                    WriteLine(activityInfo);
                    scanDetails.AppendLine("Live Disk Activity (bytes/sec):");
                    scanDetails.AppendLine(activityInfo);
                }
            }
            catch (Exception ex)
            {
                if (withsounds)
                {
                    Beep();
                }

                string counterError = "Read/write counters unavailable: " + ex.Message;
                WriteLine(counterError);
                scanDetails.AppendLine(counterError);
            }
        }

        // GPU METH
        static void GetGpuInfo()
        {
            if (withsounds)
            {
                Beep();
            }

            ForegroundColor = ConsoleColor.Green;
            WriteLine("\n================ GPU ================");
            ForegroundColor = ConsoleColor.Blue;
            scanDetails.AppendLine();
            scanDetails.AppendLine("GPU");

            using (ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    if (withsounds)
                    {
                        Beep();
                    }

                    string name = Convert.ToString(item["Name"]);
                    double memoryBytes = Convert.ToDouble(item["AdapterRAM"] ?? 0);

                    string gpuInfo =
                        "GPU Name : " + name + Environment.NewLine +
                        "VRAM    : " +
                        (memoryBytes / 1024.0 / 1024.0 / 1024.0).ToString("0.00") + " GB";

                    WriteLine(gpuInfo);
                    scanDetails.AppendLine(gpuInfo);
                    scanDetails.AppendLine("-------------------------------------");
                }
            }
        }

        // SYSTEM SCAN METH
        static void CheckSpecs()
        {
            ForegroundColor = ConsoleColor.Green;
            WriteLine("\nChecking System (" + DateTime.Now.ToLongTimeString() + ")");

            GetCpuInfo();
            GetRamInfo();
            GetStorageInfo();
            GetGpuInfo();

            ForegroundColor = ConsoleColor.White;
        }

        // SYSTEM OVERVIEW METH
        static void ShowSystemOverview()
        {
            if (withsounds)
            {
                Beep();
            }

            ForegroundColor = ConsoleColor.Green;
            WriteLine("\n========== SYSTEM OVERVIEW ==========");
            ForegroundColor = ConsoleColor.DarkBlue;

            WriteLine("CPU Cores       : " + cpuCores);
            WriteLine("CPU Threads     : " + cpuThreads);
            WriteLine("CPU Speed       : " + cpuSpeedGHz.ToString("0.00") + " GHz");
            WriteLine("RAM Size        : " + ramGB.ToString("0.00") + " GB");
            WriteLine("RAM Speed       : " + ramSpeedMHz + " MHz");
            WriteLine("Physical Storage: " + totalStorageGB.ToString("0.00") + " GB");
            WriteLine("Logical Storage : " + totalLogicalStorageGB.ToString("0.00") + " GB");
            WriteLine("Free Storage    : " + totalFreeStorageGB.ToString("0.00") + " GB");
            WriteLine("====================================");

            ForegroundColor = ConsoleColor.Yellow;
            Write("\nDo you wish to save the overview to desktop? Y / N: ");
            string saveChoice = ReadLine();

            if (saveChoice != null &&
                saveChoice.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                if (withsounds)
                {
                    Beep();
                }

                SaveSystemLog();
            }
        }

        // LOG METH
        static void SaveSystemLog()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktop,
                "HARDIS System Overview " +
                DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("HARDIS - COMPLETE SYSTEM SCAN REPORT");
                    writer.WriteLine("Scan Date: " + DateTime.Now);
                    writer.WriteLine();

                    writer.WriteLine("***** Key Info *****");
                    writer.WriteLine("CPU Cores             : " + cpuCores);
                    writer.WriteLine("CPU Threads           : " + cpuThreads);
                    writer.WriteLine("CPU Maximum Speed     : " + cpuSpeedGHz.ToString("0.00") + " GHz");
                    writer.WriteLine("Total RAM             : " + ramGB.ToString("0.00") + " GB");
                    writer.WriteLine("RAM Speed             : " + ramSpeedMHz + " MHz");
                    writer.WriteLine("Total Physical Storage: " + totalStorageGB.ToString("0.00") + " GB");
                    writer.WriteLine("Total Logical Storage : " + totalLogicalStorageGB.ToString("0.00") + " GB");
                    writer.WriteLine("Total Free Storage    : " + totalFreeStorageGB.ToString("0.00") + " GB");
                    writer.WriteLine();

                    writer.WriteLine("***** Additional Info *****");
                    writer.WriteLine(scanDetails.ToString());
                    writer.WriteLine();
                    writer.WriteLine("Copyright Harvey. Dwane. Aldrin. 2026");
                }

                ForegroundColor = ConsoleColor.White;
                WriteLine("Overview saved successfully: " + filePath);
                WriteLine();

                ForegroundColor = ConsoleColor.Cyan;
                WriteLine("Thank you for using Hardis :)");
            }
            catch (Exception ex)
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Unable to save overview: " + ex.Message);
            }

            ForegroundColor = ConsoleColor.White;
        }

        // UTILITY METH
        static string FormatBytesPerSecond(double bytes)
        {
            if (bytes < 1024)
            {
                return bytes.ToString("0.00") + " B/s";
            }

            if (bytes < 1024 * 1024)
            {
                return (bytes / 1024.0).ToString("0.00") + " KB/s";
            }

            if (bytes < 1024 * 1024 * 1024)
            {
                return (bytes / 1024.0 / 1024.0).ToString("0.00") + " MB/s";
            }

            return (bytes / 1024.0 / 1024.0 / 1024.0).ToString("0.00") + " GB/s";
        }
    }
}