# DevOp
training

"- Viết dockerfile để tạo 1 custom image của nginx.
- Thay đổi nội dung của file html trong thư mục html bằng tên của mình
- Deploy container lên host, map cổng 80, 443 của container với cổng 80, 443 của host
"	Truy cập tên miền đã đăng kí, hiển thị "Xin chào {Tên của bạn}
- Viết docker compose để deploy 1 container của Redis, có cấu hình password, cổng của redis là 7484	Dùng phần mềm "Another redis desktop manager", truy cập vào redis theo cấu hình đã cài đặt, truy cập thành công, có thể edit được dữ liệu
- Viết docker compose để deploy 1 container của emqx, có cấu hình user name và password cho emqx	Dùng phần mềm mqtt client (vd MQTT Box), kết nối vào container thành công, pub, sub được topic
"1. Viết 1 API cơ bản (có thể dùng ngôn ngữ gì tùy ý, khuyến khích dùng .Net core), sao cho khi chạy, truy cập vào đường dẫn API (vd localhost:8080/api/hello) hiển thị dòng chữ: ""Xin chào API""
2. Viết docker file để build image chương trình trên.
3. Push docker image lêndoc docker hub.
4. Trên máy chủ EC2, pull docker image này về và chạy trên máy chủ, tạo sub domain, cấu hình nginx proxy để trỏ tới service này, sao cho khi truy cập với domain đã tạo ở bài trước (vd: https://api.domain.xyz/api/hello) thì hiển thị dòng chữ ""Xin chào API"""	"1. Trên server, dùng lệnh ""docker ps"", thấy có api đang chạy.
2. Truy cập vào sub domain (vd: https://api.domain.xyz/api/hello) thì hiển thị dòng chữ ""Xin chào API"""