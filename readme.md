# MqttPowerState

Very simple C# app that sends either a "wake" or "sleep" message to the "windows" channel of an MQTT server.

Arguments must be either "wake" or "sleep".

Environment variables must be set:

* MQTT_USERNAME
* MQTT_PWD
* MQTT_HOST
* MQTT_PORT

No TLS support right now.