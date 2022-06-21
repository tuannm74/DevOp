Đề bài
1. Cài đặt K8S lên máy ảo
  => Cài đặt thành công lên 1 cụm k8s gồm 1 master và 2 worker
3. Deploy các service EMQX, RabbitMQ lên K8S
  => Deploy thành công
5. Deploy các pod đã viết ở bài trước lên K8S, kết nối với EMQX và RabbitMQ ở trên\
  => Sau khi publish message lên MQTT, pod đang subcribe exchange RabbitMQ sẽ hiển thị cảnh báo nếu ngưỡng pin nhỏ hơn 10%
7. Deploy các pod dưới dạng Deployment, Replicaset
  => Deploy thành công
9. Cài đặt K8S dashboard và grafana để monitoring hệ thống
  => Giám sát được tình trạng các pod, các node, các service trong cụm k8s
