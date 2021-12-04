using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Amazon.CognitoIdentity;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using Amazon.S3.Util;
using System.Runtime.Serialization.Formatters.Binary;

public class AwsManager : MonoBehaviour
{
    private static AwsManager _instance;
    public static AwsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("aws instance is null and not assigned");

            }
            return _instance;
        }
    }

    public string S3Region = RegionEndpoint.USEast2.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }


    private AmazonS3Client s3client;
    public AmazonS3Client S3client
    {
        get
        {
            if (s3client == null)
            {
                s3client = new AmazonS3Client(new CognitoAWSCredentials(
                "us-east-2:287d17a0-d361-494e-a452-31a2f99a47ed", // Identity Pool ID
                RegionEndpoint.USEast2 // Region
                ), _S3Region);

            }

            return s3client;
        }
    }
    public void Awake()
    {
        _instance = this;
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

     
       // to get the list of bucket 
       /* S3client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        {
           
            if (responseObject.Exception == null)
            {
                
                responseObject.Response.Buckets.ForEach((s3b) =>
                {
                    Debug.Log("bucket list " + s3b.BucketName);
                });
            }
            else
            {
                Debug.Log("bnucket not found" + responseObject.Exception);
            }
        });*/
    }

    public void UploadToS3(string path,string filename)
    {
        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = "serviceappcasebucket",
            Key = "case#" + filename,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        S3client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                Debug.Log("file uploaded to the bucket ");
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.Log("exception occure file is not uploaded" + responseObj.Exception);
            }
        });
    }

    public void GetList(string casenumber, Action oncomplete = null)
    {
        string target = "case#" + casenumber;
        var request = new ListObjectsRequest()
        {
            BucketName = "serviceappcasebucket"
        };

        S3client.ListObjectsAsync(request, (responseObject) =>
        {
            
            if (responseObject.Exception == null)
            {
                bool casefound = responseObject.Response.S3Objects.Any(obj => obj.Key == target);
                if (casefound)
                {
                    Debug.Log("Case found");
                    S3client.GetObjectAsync("serviceappcasebucket", target, (responseobj) =>
                      {
                          //read the data and apply to Case(object) to be used

                          // check the responsestream is not null

                          if(responseobj.Response.ResponseStream != null)
                          {
                              //byte  array to store data from file
                              byte[] data = null;

                              //use streamReader to read the response  data 
                              using(StreamReader reader = new StreamReader(responseobj.Response.ResponseStream))
                              {
                                  //acces memory stream 

                                  using(MemoryStream memory = new MemoryStream())
                                  {
                                      //populate data byte array with menstream data 
                                      var buffer = new byte[512];
                                      var byteRead = default(int);

                                      while((byteRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                                      {
                                          memory.Write(buffer, 0, byteRead);
                                      }
                                      data = memory.ToArray();
                                  }
                                  
                              }

                              //convert those bytes to case object 
                              using (MemoryStream memory = new MemoryStream(data))
                              {
                                  BinaryFormatter bf = new BinaryFormatter();
                                  Case downloadedcase = (Case)bf.Deserialize(memory);

                                  UIManager.Intance.activecase = downloadedcase;

                                  if (oncomplete != null)
                                      oncomplete();
                              }

                          }
                      });

                }
                else
                {
                    Debug.Log("Case Not Found");
                }
                
               
            }
            else
            {
                Debug.Log("error in getting list " + responseObject.Exception);
            }
        });
    }
   
}
