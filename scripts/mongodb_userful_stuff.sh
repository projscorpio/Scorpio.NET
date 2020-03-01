sudo apt-get install mongodb
cat /etc/mongodb.conf
sudo systemctl status mongodb.service
mongo --port 27017
use admin
db.createUser({user: 'mongodbuser', pwd: 'mongodbpassword', roles: ["root"]})
