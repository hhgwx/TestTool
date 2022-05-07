
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressFolder
{
    class ICSharpZip
    {
		/// <summary>
		/// 压缩文件
		/// </summary>
		/// <param name="sourceFilePath"></param>
		/// <param name="destinationZipFilePath"></param>
		/// <param name="password"></param>
		public static string CreateZip(string sourceFilePath, string destinationZipFilePath, string password)
		{
//			if (sourceFilePath[sourceFilePath.Length - 1] != '/')
//				sourceFilePath += "/";
            string pathTemp = "";
            if (sourceFilePath.EndsWith("/"))
            {
                pathTemp = sourceFilePath.Substring(0, sourceFilePath.Length-1);
                pathTemp = pathTemp.Substring(0, pathTemp.LastIndexOf("/"));
            } else
            {
                pathTemp = pathTemp.Substring(0, pathTemp.LastIndexOf("/"));
            }

            try
            {
                FastZip fastZip = new FastZip();
                if (password != null && password != "")
                {
                    fastZip.Password = password;
                }
                fastZip.CreateEmptyDirectories = true;
                fastZip.NameTransform = new ZipNameTransform(pathTemp);
                fastZip.CreateZip(destinationZipFilePath, sourceFilePath, true, "");
                return "OK";
            }
            catch(Exception)
            {
				return "false";
			}

            /*
ZipOutputStream zipStream = null;
			try
			{
				zipStream = new ZipOutputStream(File.Create(destinationZipFilePath));
				if (password!=null && password != "") {
					zipStream.Password = password;
				}
				
				zipStream.SetLevel(level);  // 压缩级别 0-9
				CreateZipFiles(sourceFilePath, zipStream, sourceFilePath.Substring(0, sourceFilePath.LastIndexOf("/")));
			}
			catch (Exception)
			{
				return "NG";
			}
			finally
			{
				if (zipStream != null) {
					zipStream.Finish();
					zipStream.Close();
				}
			}

			return "OK";*/
		}

		/// <summary>
/*		/// 递归压缩文件
		/// </summary>
		/// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
		/// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名</param>
		/// <param name="staticFile"></param>
				private static void CreateZipFiles (string sourceFilePath, ZipOutputStream zipStream, string staticFile) 
				{
					Crc32 crc = new Crc32();
					string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath);
					foreach (string file in filesArray)
					{
						if (Directory.Exists(file))                     //如果当前是文件夹，递归
						{
							CreateZipFiles(file + "/", zipStream, staticFile);
						}

						else                                            //如果是文件，开始压缩
						{
							FileStream fileStream = File.OpenRead(file);

							byte[] buffer = new byte[fileStream.Length];
							fileStream.Read(buffer, 0, buffer.Length);
							string tempFile = file.Substring(staticFile.LastIndexOf("/") + 1);
							ZipEntry entry = new ZipEntry(tempFile);
							entry.IsUnicodeText = true;

							entry.DateTime = DateTime.Now;
							entry.Size = fileStream.Length;
							fileStream.Close();
							crc.Reset();
							crc.Update(buffer);
							entry.Crc = crc.Value;
							zipStream.PutNextEntry(entry);

							zipStream.Write(buffer, 0, buffer.Length);
						}
					}
				}*/

		/// <summary>
		/// 解压缩文件
		/// </summary>
		/// <param name="zipFilePath"></param>
		/// <param name="toPath"></param>
		/// <param name="password"></param>
		public static string UnZip(string zipFilePath, string toPath, string password, Boolean overWrite)
		{
			try
			{
				FastZip fastZip = new FastZip();
				if (password != null && password != "")
				{
					fastZip.Password = password;
				}
				if (overWrite)
				{
					fastZip.ExtractZip(zipFilePath, toPath, FastZip.Overwrite.Always, null, "", "", false);
				}
				else
				{
					fastZip.ExtractZip(zipFilePath, toPath, FastZip.Overwrite.Never, null, "", "", false);
				}
				return "OK";
			}
			catch (Exception)
			{ return "false"; }
		}
	}
}
