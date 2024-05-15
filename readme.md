# Desciption
Backend for the phishing site task of stem games technology arena 2024 day 3

## Functions
It consists of 2 parts:
	- REST API
	- Database
	
# parts
## REST API
Rest API is a rudimentary rest API that handles requests to the server, it is located in the ./Server/HttpServer.cs

## Database
Everything else is database code, it saves tables as folders, and each instance as a file.

It hashes the passwords with SHA512

### Database file structure
The database saves everything as either a byte or a byte array, byte being saved as just a byte value, and byte arrays being saved as a 16bit length N followed by N bytes representing values

Multi-byte types are saved as an array of N bytes (e.g. integer is stored as 4 byte array, meaning it takes 6 bytes in the file)

Strings are saved as UTF-8 encoded byte arrays.

Lists are stored recursively, first the size of the whole List, then sizes of each type however the type was meant to be saved

In the Encoders folder there are all the encoders in ByteEncoder.cs, all the decoders in ByteDecoder.cs, along with password hasher in PasswordHasher.cs


# What we failed
At the moment we only have Register and loging implemented in the reSt API, and while the database supports events, they aren't fully implemented

There is an error where the content type comes as null and content body is blank in the request, we haven't managed to figure out if it's on the frontend side or the backend.