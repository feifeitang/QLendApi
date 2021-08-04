version=1.0.0
name=qlend

build:
	docker build -t qlend:${version} .
tag:
	docker tag qlend:${version} qlend.azurecr.io/qlend:${version}
push:
	docker push qlend.azurecr.io/qlend:${version}
