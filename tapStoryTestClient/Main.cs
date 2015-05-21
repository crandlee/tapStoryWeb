using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace tapStoryTestClient
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void SendDataButton_Click(object sender, EventArgs e)
        {
            var oRequest = (HttpWebRequest)WebRequest.Create(UrlText.Text);
            var boundary = Guid.NewGuid().ToString();
            oRequest.Headers.Add("Authorization", String.Format("Bearer {0}", TokenText.Text));
            oRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            oRequest.Method = "GET";            
            if (methodPost.Checked) oRequest.Method = "POST";
            if (methodPut.Checked) oRequest.Method = "PUT";
            if (methodPatch.Checked) oRequest.Method = "PATCH";
            if (methodDelete.Checked) oRequest.Method = "DELETE";
            
            var pData = new PostData();
            var encoding = Encoding.UTF8;
            Stream oStream = null;

            if (!String.IsNullOrEmpty(File1Text.Text))
            {
                var fn = Path.GetFileName(File1Text.Text);
                pData.Params.Add(new PostDataParam(fn, File1Text.Text, ReadFile(File1Text.Text), PostDataParamType.File));
            }
            if (!String.IsNullOrEmpty(File2Text.Text))
            {
                var fn = Path.GetFileName(File2Text.Text);
                pData.Params.Add(new PostDataParam(fn, File2Text.Text, ReadFile(File2Text.Text), PostDataParamType.File));
            }
            if (!String.IsNullOrEmpty(File3Text.Text))
            {
                var fn = Path.GetFileName(File3Text.Text);
                pData.Params.Add(new PostDataParam(fn, File3Text.Text, ReadFile(File3Text.Text), PostDataParamType.File));
            }
            if (!String.IsNullOrEmpty(GroupNameText.Text))
            {
                pData.Params.Add(new PostDataParam("GroupName", GroupNameText.Text, PostDataParamType.Field));
            }
            if (!String.IsNullOrEmpty(UserIdText.Text))
            {
                pData.Params.Add(new PostDataParam("UserId", UserIdText.Text, PostDataParamType.Field));
            }
            if (!String.IsNullOrEmpty(FileGroupServerIdText.Text))
            {
                pData.Params.Add(new PostDataParam("FileGroupServerId", FileGroupServerIdText.Text, PostDataParamType.Field));
            }

            /* ... set the parameters, read files, etc. IE:
               pData.Params.Add(new PostDataParam("email", "example@example.com", PostDataParamType.Field));
               pData.Params.Add(new PostDataParam("fileupload", "filename.txt", "filecontents" PostDataParamType.File));
            */


            var buffer = encoding.GetBytes(pData.GetPostData(boundary));

            oRequest.ContentLength = buffer.Length;
            RequestText.Text = Encoding.Default.GetString(buffer);
            oStream = oRequest.GetRequestStream();
            oStream.Write(buffer, 0, buffer.Length);
            oStream.Close();
            try
            {
                var oResponse = (HttpWebResponse) oRequest.GetResponse();
                var respEncoding = Encoding.UTF8;
                var respText = String.Empty;
                var stm = oResponse.GetResponseStream();
                if (stm != null) { 
                    using (var reader = new StreamReader(stm, respEncoding))
                    {
                        respText = reader.ReadToEnd();
                    }
                }
                ResponseText.Text = String.Format("{0}:{1}\n{2}", oResponse.StatusCode, oResponse.StatusDescription, respText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private static string ReadFile(string fileName)
        {
            try
            {
                using (var sr = new StreamReader(fileName))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("Could not read file {0}", fileName));
            }
            return string.Empty;
        }

        private void TokenText_TextChanged(object sender, EventArgs e)
        {

        }

        private void Browse1Button_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            var result = ofd.ShowDialog(); // Show the dialog.
            if (result != DialogResult.OK) return;
            var file = ofd.FileName;
            try
            {
                File1Text.Text = file;
            }
            catch (IOException)
            {
            }
        }

        private void Browse2Button_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            var result = ofd.ShowDialog(); // Show the dialog.
            if (result != DialogResult.OK) return;
            var file = ofd.FileName;
            try
            {
                File2Text.Text = file;
            }
            catch (IOException)
            {
            }

        }

        private void Browse3Button_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            var result = ofd.ShowDialog(); // Show the dialog.
            if (result != DialogResult.OK) return;
            var file = ofd.FileName;
            try
            {
                File3Text.Text = file;
            }
            catch (IOException)
            {
            }

        }

    }
}
