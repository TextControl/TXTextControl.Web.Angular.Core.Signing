const PROXY_CONFIG = [
  {
    context: [
      "/document",
    ],
    target: "https://localhost:7275",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
