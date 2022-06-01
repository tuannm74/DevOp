FROM nginx:1.20-alpine

RUN rm /etc/nginx/conf.d/default.conf

COPY nginx.conf /etc/nginx/conf.d/default.conf
	
COPY index.html /usr/share/nginx/html

RUN apk add python3 python3-dev py3-pip build-base libressl-dev musl-dev libffi-dev rust cargo

RUN pip3 install pip --upgrade

RUN pip3 install certbot-nginx
