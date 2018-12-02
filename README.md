# PPT2Web

Service and tools for exporting your Powerpoint presentation as a Web page. I built this tool for sharing decks outside my organization for UX research purposes.
It is currently a work in progress, a lot of improvements are still ongoing.

PPT2Web is composed of 3 components:
- A Powerpoint add-in for triggering the export to a web-deck and managing the URL (copying to clipboard, opening in browser).
- A Web API for uploading and serving the web-decks.
- A Web app which serves as the front-end for viewing the web-decks. It provides simple navigation through the deck as well as displaying the slide comments.