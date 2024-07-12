using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using Ionic.Zlib;

public class MioZip : MonoBehaviour
{
	public string path;
	public string outFolder;
    public float percent = 0f;

	public MioZip(string path,string outFolder){
		this.path = path;
		this.outFolder = outFolder;
	}

	void Update(){
		//if (percent > 0f && percent < 100f)
			//Debug.Log (percent);
	}

    void Start()
    {
        ZipConstants.DefaultCodePage = 0;
        //Bruteforce Windows Zip
        //CreateSample(Application.persistentDataPath + "/androidBundle.zip", null,Application.persistentDataPath + "/Android");
        //NewUnzip(this, Application.persistentDataPath + "/androidBundle.zip", Application.persistentDataPath + "/dir", delegate (float f) { }, delegate (string s) { });
        //End
    }

    // Compresses the files in the nominated folder, and creates a zip file on disk named as outPathname.
    //
    public void CreateSample(string outPathname, string password, string folderName)
    {

        FileStream fsOut = File.Create(outPathname);
        ZipOutputStream zipStream = new ZipOutputStream(fsOut);

        zipStream.SetLevel(4); //0-9, 9 being the highest level of compression

        zipStream.Password = password;  // optional. Null is the same as not setting. Required if using AES.

        // This setting will strip the leading part of the folder path in the entries, to
        // make the entries relative to the starting folder.
        // To include the full path for each entry up to the drive root, assign folderOffset = 0.
        int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

        CompressFolder(folderName, zipStream, folderOffset);

        zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
        zipStream.Close();
    }

    // Recurses down the folder structure
    //
    private void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
    {

        string[] files = Directory.GetFiles(path);

        foreach (string filename in files)
        {

            FileInfo fi = new FileInfo(filename);

            string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
            entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
            ZipEntry newEntry = new ZipEntry(entryName);
            newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

            // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
            // A password on the ZipOutputStream is required if using AES.
            //   newEntry.AESKeySize = 256;

            // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
            // you need to do one of the following: Specify UseZip64.Off, or set the Size.
            // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
            // but the zip will be in Zip64 format which not all utilities can understand.
            //   zipStream.UseZip64 = UseZip64.Off;
            newEntry.Size = fi.Length;

            zipStream.PutNextEntry(newEntry);

            // Zip the file in buffered chunks
            // the "using" will close the stream even if an exception occurs
            byte[] buffer = new byte[4096];
            using (FileStream streamReader = File.OpenRead(filename))
            {
                StreamUtils.Copy(streamReader, zipStream, buffer);
            }
            zipStream.CloseEntry();
        }
        string[] folders = Directory.GetDirectories(path);
        foreach (string folder in folders)
        {
            CompressFolder(folder, zipStream, folderOffset);
        }
    }

    public static void NewUnzip(MonoBehaviour mono, string path, string outFolder, Action<float> onProgress, Action<string> onComplete){

		if(SystemInfo.operatingSystem.Contains ("Windows"))
			mono.StartCoroutine(ExtractZipFile2(path, outFolder, onProgress, onComplete));
		else
			mono.StartCoroutine(ExtractZipFileBundle(path, outFolder, onProgress, onComplete));

	}

	public static IEnumerator ExtractZipFile2(string path, string outFolder, Action<float> onProgress, Action<string> onComplete)
    {
		Debug.Log ("inizio unzip");

        ZipFile zf = null;

        long fileLength = new System.IO.FileInfo(path).Length;
        long unzipped = 0;
        Debug.Log("File length:" + fileLength);

        try
        {
            Debug.Log("Sono qui 1");
            FileStream fs = File.OpenRead(path);
            Debug.Log("Sono qui 2");


            byte[] zipFileData = File.ReadAllBytes(path);

            MemoryStream fileStream = new MemoryStream();
           
                Debug.Log("Sono qui 3");
                fileStream.Write(zipFileData, 0, zipFileData.Length);
                fileStream.Flush();
                fileStream.Seek(0, SeekOrigin.Begin);
                Debug.Log("Sono qui 4");

                zf = new ZipFile(fileStream);




                zf = new ZipFile(fs);
            Debug.Log("Sono qui 3");

            foreach (ZipEntry zipEntry in zf)
            {
                if (!zipEntry.IsFile)
                {
                    continue;           // Ignore directories
                }
                String entryFileName = zipEntry.Name;
                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                unzipped += zipEntry.Size;

                double unzippedD = unzipped / 1024.0;
                double fileLengthD = fileLength / 1024.0;

                float unzippedF = (float)unzippedD;
                float fileLengthF = (float)fileLengthD;

				float percent = (unzippedF / fileLengthF) * 100;
				onProgress((unzippedF / fileLengthF));

                //Debug.Log(percent);

                byte[] buffer = new byte[4096];     // 4K is optimum
                Stream zipStream = zf.GetInputStream(zipEntry);

                // Manipulate the output filename here as desired.
                String fullZipToPath = Path.Combine(outFolder, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
					//Debug.Log(fullZipToPath);
                    //yield return new WaitForSeconds(0.1f);
                }
            }
        }
        finally
        {
            if (zf != null)
            {
                zf.IsStreamOwner = true; // Makes close also shut the underlying stream
				zf.Close(); // Ensure we release resource;
            }
        }
		onComplete("UnZip Completato");
        yield return 0;
    }

    public void WindowsMerda()
    {

        string startPath = Application.persistentDataPath + "/Android";
        string zipPath = Application.persistentDataPath + "/androidBundle.zip";
        string extractPath = Application.persistentDataPath + "/dir/WindowsMerda";

        // ZipFile.CreateFromDirectory(startPath, zipPath);

        //ZipFile.ExtractToDirectory(zipPath, extractPath);
    }

	public static IEnumerator ExtractZipFileBundle(string path, string outFolder, Action<float> onProgress, Action<string> onComplete)
    {

        ZipFile zf = null;

        long fileLength = new System.IO.FileInfo(path).Length;
        long unzipped = 0;
        Debug.Log("File length:" + fileLength);

        try
        {
            FileStream fs = File.OpenRead(path);
            zf = new ZipFile(fs);
            foreach (ZipEntry zipEntry in zf)
            {
                if (!zipEntry.IsFile)
                {
                    continue;           // Ignore directories
                }
                String entryFileName = zipEntry.Name;
                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                unzipped += zipEntry.Size;

                double unzippedD = unzipped / 1024.0;
                double fileLengthD = fileLength / 1024.0;

                float unzippedF = (float)unzippedD;
                float fileLengthF = (float)fileLengthD;

                //percent = (unzippedF / fileLengthF) * 100;
				onProgress((unzippedF / fileLengthF));

                //Debug.Log(percent);

                byte[] buffer = new byte[4096];     // 4K is optimum
                Stream zipStream = zf.GetInputStream(zipEntry);

                // Manipulate the output filename here as desired.
                String fullZipToPath = Path.Combine(outFolder, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
                    Debug.Log(fullZipToPath);
                    //yield return new WaitForSeconds(0.1f);
                }
            }
        }
        finally
        {
            if (zf != null)
            {
                zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                zf.Close(); // Ensure we release resources
            }
        }
		yield return new WaitForSeconds(0.1f);
		onComplete("UnZip Completato");
        yield return 0;
    }

    public IEnumerator ExtractZipFile(){
		
		ZipFile zf = null;

		long fileLength = new System.IO.FileInfo(path).Length;
		long unzipped = 0;
		Debug.Log ("File length:" + fileLength);

		try {
			FileStream fs = File.OpenRead(path);
			zf = new ZipFile(fs);
			foreach (ZipEntry zipEntry in zf) {
				if (!zipEntry.IsFile) {
					continue;           // Ignore directories
				}
				String entryFileName = zipEntry.Name;
				// to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
				// Optionally match entrynames against a selection list here to skip as desired.
				// The unpacked length is available in the zipEntry.Size property.

				unzipped += zipEntry.Size;

				double unzippedD = unzipped/1024.0;
				double fileLengthD =fileLength/1024.0;

				float unzippedF =(float) unzippedD;
				float fileLengthF =(float)fileLengthD;

				percent = (unzippedF/fileLengthF)*100;

				//Debug.Log(percent);

				byte[] buffer = new byte[4096];     // 4K is optimum
				Stream zipStream = zf.GetInputStream(zipEntry);

				// Manipulate the output filename here as desired.
				String fullZipToPath = Path.Combine(outFolder, entryFileName);
				string directoryName = Path.GetDirectoryName(fullZipToPath);
				if (directoryName.Length > 0)
					Directory.CreateDirectory(directoryName);

				// Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
				// of the file, but does not waste memory.
				// The "using" will close the stream even if an exception occurs.
				using (FileStream streamWriter = File.Create(fullZipToPath)) {
					StreamUtils.Copy(zipStream, streamWriter, buffer);
					Debug.Log(fullZipToPath);
					//yield return new WaitForSeconds(0.1f);
				}
			}
		} finally {
			if (zf != null) {
				zf.IsStreamOwner = true; // Makes close also shut the underlying stream
				zf.Close (); // Ensure we release resources
			}
		}
		yield return 0;
	}



    public byte[] FileToByteArray(string fileName)
    {
        byte[] buff = null;
        FileStream fs = new FileStream(fileName,
                                       FileMode.Open,
                                       FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(fileName).Length;
        buff = br.ReadBytes((int)numBytes);
        return File.ReadAllBytes(fileName);
    }





    public void decompress(WWW download)
    {
        Debug.Log("Decompress");
        byte[] data = download.bytes;
        byte[] buffer ;

        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        string textAsset = "";

        if (data != null)
        {
            int total_read= 0;

            Stream zip_stream;
            var input_stream = new MemoryStream(data);
            zip_stream = new DeflateStream(input_stream, CompressionMode.Decompress, true);

            int read= -1;
            buffer = new byte[4096];
            while (read != 0)
            {
                read = zip_stream.Read(buffer, total_read, buffer.Length);
                textAsset += encoding.GetString(buffer, total_read, total_read);

                total_read += read;
            }

            Debug.Log(encoding.GetString(buffer, 0, total_read));
            Debug.Log(total_read);
        }
        else
        {
            Debug.Log("Couldnt load resource");
        }

    }






    public static void UnZipWindowsMerda(string filePath, byte[] data)
    {
        using (ZipInputStream s = new ZipInputStream(new MemoryStream(data)))
        {
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
#if UNITY_EDITOR
                Debug.LogFormat("Entry Name: {0}", theEntry.Name);
#endif

                string directoryName = Path.GetDirectoryName(theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);

                // create directory
                if (directoryName.Length > 0)
                {
                    var dirPath = Path.Combine(filePath, directoryName);

#if UNITY_EDITOR
                    Debug.LogFormat("CreateDirectory: {0}", dirPath);
#endif

                    Directory.CreateDirectory(dirPath);
                }

                if (fileName != string.Empty)
                {
                    // retrieve directory name only from persistence data path.
                    var entryFilePath = Path.Combine(filePath, theEntry.Name);
                    using (FileStream streamWriter = File.Create(entryFilePath))
                    {
                        int size = 2048;
                        byte[] fdata = new byte[size];
                        while (true)
                        {
                            size = s.Read(fdata, 0, fdata.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(fdata, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            } //end of while
        } //end of using
    }
}





