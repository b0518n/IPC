using System.IO.Pipes;

namespace NamedPipeClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "mypipe", PipeDirection.InOut))
                {
                    Console.WriteLine("클라이언트 : 서버에 연결을 시도 중...");
                    pipeClient.Connect();
                    Console.WriteLine("클라이언트 : 서버에 연결 성공!");

                    string message;
                    string response;

                    using (StreamReader sr = new StreamReader(pipeClient))
                    using (StreamWriter sw = new StreamWriter(pipeClient))
                    {
                        while(true)
                        {
                            Console.WriteLine("클라이언트 : 메시지 입력 (exit로 종료)");
                            message = Console.ReadLine();
                            if (message == "exit")
                            {
                                break;
                            }

                            sw.WriteLine(message);
                            sw.Flush();

                            response = sr.ReadLine();
                            Console.WriteLine($"클라이언트 : 받은 응답 - {response}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"클라이언트 에러 : {ex.Message}");
            }

            Console.WriteLine("클라이언트 : 종료됨");
        }
    }
}