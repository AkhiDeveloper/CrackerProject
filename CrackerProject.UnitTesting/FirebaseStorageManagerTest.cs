
using CrackerProject.API.Data.Firebase.Storage;
using CrackerProject.API.Data.MongoDb.SchemaOne.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackerProject.UnitTesting
{
    public class FirebaseStorageManagerTest
    {

        [Fact]
        public void Check_FirebaseConfiguration()
        {
            //Arrange
            var config=new API.Settings.FirebaseConfig();
            string apikey = "AIzaSyB_fp1PQTXUlZH8_-btXeHwgqJWYtipPN";

            //Assert
            Assert.True(config.ApiKey == apikey);
            //Assert.False(string.IsNullOrEmpty(config.ApiKey));

        }
        [Fact]
        public void Check_UploadFile()
        {
            //Arrange
            FileInfo file = new FileInfo("E:\\akhilesh\\Certificate\\PP Size Photo.jpeg");
            Stream filestream = file.OpenRead();
            var manager = new FirebaseStorageManager(new API.Settings.FirebaseConfig());

            //Act
            //string output = manager.UploadFile(filestream)).Result;

            //Assert
            //Assert.True(output != null);
        }
    }
}
