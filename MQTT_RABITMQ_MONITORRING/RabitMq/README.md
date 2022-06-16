1.Tạo image chương trình pub mes:

       docker build -t tuannm74/rabit:1.0

       docker push tuannm74/rabit:1.0

       e2c pull: docker pull tuannm74/rabit:1.0
2. Tạo rabitMq bằng docker-compose

      docker-compose up -d
      
      
  MQTT BOX:
      Gửi mes topic: devops/data 
      Pay load: {baterry:88}
      
  Xem log server :
    file images
      
