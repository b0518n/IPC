using System.IO.Pipes;

namespace NamedPipeServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // PipeDirection : 파이프의 방향을 지정
                // PipeDirection.In : 읽기 전용(단방향)
                // PipeDirection.Out : 쓰기 전용(단방향)
                // PipeDirection.InOut : 양방향
                using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("mypipe", PipeDirection.InOut))
                {
                    Console.WriteLine("서버 : 파이프를 생성하고 클라이언트를 기다리는 중...");
                    pipeServer.WaitForConnection(); // 동기식
                    Console.WriteLine("서버 : 클라이언트와 연결 성공!");

                    string message;
                    string response;

                    using (StreamReader sr = new StreamReader(pipeServer))
                    using (StreamWriter sw = new StreamWriter(pipeServer))
                    {
                        while (true)
                        {
                            message = sr.ReadLine();
                            if (message == "exit")
                            {
                                break;
                            }

                            Console.WriteLine($"서버 : 받은 메시지 - {message}");

                            // 서버에서 클라이언트로 메시지 전송
                            response = "서버가 받은 메시지: " + message;
                            sw.WriteLine(response);
                            sw.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"서버 에러: {ex.Message}");
            }

            Console.WriteLine("서버 : 종료됨");
        }
    }
}