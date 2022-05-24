using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SharedLibrary.Dtos
{
    public class Response<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        public ErrorDto Error { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }
        public static Response<T> Success(T data, int statusCode)//hem status code hemde datasini verecek
        {
            return new Response<T> { Data = data, StatusCode = statusCode ,IsSuccessful=true};
            //urun ekleme isleminde data donmemiz gerekiyor cunku ilgili data eklediginde id verilecek
        }
        public static Response<T> Success(int statusCode) //data bos olacak ama status codu olacak
        {
            return new Response<T> {Data=default ,StatusCode = statusCode,IsSuccessful=true }; //error verdigi zaman gosterilecek 
            //urunu update ettigiimzde yada urunu sildigimizde bu endpointlerde bir data donmeye gerek yok
        }


        //generic oldugundan fail de yine de <T> yi vermemiz gerekiyor
        public static Response<T> Fail(int statusCode,ErrorDto errorDto)
        {
            return new Response<T> { Error = errorDto, StatusCode = statusCode ,IsSuccessful=false};

        }
        public static Response<T> Fail(string errorMessage,int statusCode,bool isShow)//tek bir mesajida kisa yoldan yollsin diye bunu yazdik
        {
            var errorDta=new ErrorDto(errorMessage,isShow);
            return new Response<T> { Error = errorDta, StatusCode = statusCode,IsSuccessful=false };   
        }
    }
}
