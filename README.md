This is a [Next.js](https://nextjs.org/) project bootstrapped with [`create-next-app`](https://github.com/vercel/next.js/tree/canary/packages/create-next-app).
a Simple UI for a Gateways's handling web API.

## Getting Started

First, you need to change the following enviroment variables according to where are you hosting the Web API

NEXT_PUBLIC_apiIP= <<ip address>>
NEXT_PUBLIC_apiPort= <<Https Port>>

By default NextJS run on port 3000, if you need to change this modify package.json file scripts section:

    "dev": "next dev",          =>     "dev": "next dev -p PORT_YOU_LIKE", 
    "start": "next start",      =>     "dev": "next start -p PORT_YOU_LIKE", 

## Production

To build the UI for production run the build scripts

npm run buid

