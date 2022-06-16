1.Tạo image chương trình pub mes:

       docker build -t tuannm74/rabit:1.0

       docker push tuannm74/rabit:1.0

       e2c pull: docker pull tuannm74/rabit:1.0
       
       docker run -d -p 5127:5127 --name rabitmqSub tuannm74/rabit:1.0
2. Tạo rabitMq bằng docker-compose

       docker-compose up -d
      
3. MQTT BOX: 18.144.168.248:1883

       Gửi mes topic: devops/data 
       Pay load: {baterry:8}
      
4. Xem log container server :
        => hiển thị pin yếu
      
