1. Tạo image chương trình pub mes:

        docker build -t tuannm74/mqtt:1.0

        docker push tuannm74/mqtt:1.0

        e2c pull: docker pull tuannm74/mqtt:1.0
        
        docker run -d -p 5024:5024 --name mqttPub tuannm74/mqtt:1.0

3. Tạo mqtt bằng docker-compose

        docker-compose up -d
