Đề bài:

1- Viết docker file để chạy rabbitmq và MQTT trên máy chủ EC2: 
    => Truy cập trang https://rabbit.{my domain}.xyz, https://mqtt.{my domain}.xyz thành công
    Có thể truy cập mqtt thông qua các giao thức có SSL như WSS, MQTTS.
    Subcribe, Publish được message
  
2- Viết 1 chương trình X, (ngôn ngữ lập trình tùy ý, khuyến khích dùng .net core) để:
    - Nhận dữ liệu (json) từ topic mqtt: "devops/data", cấu trúc dữ liệu như sau:
    {
     "battery": 80
    }
    - Khi payload có giá trị "battery" < 10 thì gửi 1 message lên RabbitMQ, exchange type là fanout
    Viết 1 chương trình Y, subcribe exchange ở trên, sao cho khi chương trình X đẩy dữ liệu lên exchange thì chương trình Y nhận được thông tin và hiển thị lên màn hình thông báo: "Cảnh báo ngưỡng acquy ở mức thấp: 10%"
    Deploy 2 chương trình lên server EC2 thông qua docker compose
    => Publish payload MQTT với các ngưỡng battery khác nhau, watch log của chương trình Y trên console thấy thông tin cảnh báo nếu ngưỡng acquy ở mức thấp
  
3- Viết docker compose để cài đặt image prometheus và grafana, cài đặt agent để thu thập dữ liệu resource (CPU, RAM, HDD) của server EC2.
    Cài đặt alert trên giao diện grafana để khi resource (CPU, RAM, HDD) vượt quá ngưỡng cho phép thì gửi notification đến telegram
   => Truy cập vào đường dẫn "https://monitoring.domain.xyz", hiển thị thông tin resource của server: CPU, RAM
   
4- Cài đặt agent (exporter) để thu thập dữ liệu của MQTT và RabbitMQ (, config prometheus để collect các dữ liệu này, hiển thị lên dashboard.
    => Truy cập vào đường dẫn "https://monitoring.domain.xyz", hiển thị thông tin MQTT, RabbitMQ trên dashboard

