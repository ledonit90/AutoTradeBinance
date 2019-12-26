using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricesWs.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using System.IO;
    using System.Threading;

    namespace VGR.HRM.DataHub.Common
    {
        public class FileNFolderHelper
        {
            static ReaderWriterLock rwl = new ReaderWriterLock();
            // Statistics.
            static int readerTimeouts = 1;
            static int writerTimeouts = 1;
            static int reads = 0;
            static int writes = 0;
            static int timeOut = 1;
            static int FileCount = 1;

            public static void CreateFolder(string path)
            {

                try
                {
                    bool exists = Directory.Exists(path);
                    if (!exists)
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally { }
            }

            public static void MoveFile(string srcFolder, string srcFilePath, string destFolder)
            {
                try
                {
                    if (!Directory.Exists(destFolder))
                    {
                        Directory.CreateDirectory(destFolder);
                    }

                    var moveToPath = srcFilePath.Replace(srcFolder, destFolder);
                    Console.WriteLine("==========Move file " + srcFilePath + " to " + destFolder);
                    Console.WriteLine("==========Overide file " + moveToPath);
                    if (File.Exists(moveToPath))
                    {
                        File.Delete(moveToPath);
                    }
                    else
                    {
                        Console.WriteLine("==========Not existed file " + moveToPath);
                    }

                    File.Move(srcFilePath, destFolder);
                    // File.Delete(srcFilePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally { }
            }

            public static void DeleteFile(string srcFilePath)
            {
                try
                {
                    if (File.Exists(srcFilePath))
                    {
                        File.Delete(srcFilePath);
                    }
                    else
                    {
                        Console.WriteLine("==========Not existed file " + srcFilePath);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally { }
            }

            public static void WriteDatatableToFile(string path, DataTable dataTaBle)
            {
                Console.WriteLine(path);
                using (FileStream fileStream = new FileStream(path, FileMode.Append))
                {
                    StreamWriter fileWriter = new StreamWriter(fileStream, Encoding.UTF8);

                    using (fileWriter)
                    {
                        // Use sting join methods to attach and write the columns 
                        fileWriter.WriteLine(string.Join(",",
                            dataTaBle.Columns.Cast<DataColumn>().Select(csvfile =>
                                csvfile.ColumnName)));

                        foreach (DataRow row in dataTaBle.Rows)
                        {
                            // Use sting join methods to attach and write and iterate 
                            // through the rows of the datatable
                            fileWriter.WriteLine(string.Join(",", row.ItemArray));
                        }
                    }
                }
            }


            public static void WriteTextToFile(string message)
            {
                try
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string pathFile = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + "InOutService_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                    using (FileStream fileStream = new FileStream(pathFile, FileMode.Append))
                    {
                        StreamWriter fileWriter = new StreamWriter(fileStream, Encoding.UTF8);
                        using (fileWriter)
                        {
                            // Use sting join methods to attach and write the columns 
                            fileWriter.WriteLine(message);
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingHelper.LogException(e.ToString());
                }
                finally { }
            }

            // Request and release the writer lock, and handle time-outs.
            //public static void WriteTextToFile(string path, string message)
            //{
            //    Console.WriteLine("WriteTextToFile");
            //    try
            //    {
            //        rwl.AcquireWriterLock(timeOut);
            //        try
            //        {
            //            // It's safe for this thread to access from the shared resource.
            //            using (FileStream fileStream = new FileStream(path, FileMode.Append))
            //            {
            //                StreamWriter fileWriter = new StreamWriter(fileStream, Encoding.UTF8);
            //                using (fileWriter)
            //                {
            //                    // Use sting join methods to attach and write the columns 
            //                    fileWriter.WriteLine(message);
            //                }
            //            }

            //            Interlocked.Increment(ref writes);
            //        }
            //        finally
            //        {
            //            // Ensure that the lock is released.
            //            rwl.ReleaseWriterLock();
            //        }
            //    }
            //    catch (ApplicationException)
            //    {
            //        // The writer lock request timed out.
            //        Interlocked.Increment(ref writerTimeouts);
            //    }
            //}
        }
    }

}
