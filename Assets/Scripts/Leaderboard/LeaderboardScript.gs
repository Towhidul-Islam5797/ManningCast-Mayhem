// ManningCast Mayhem - Leaderboard + Contest Entry Apps Script
// Deploy this as a Web App (Deploy > New deployment > Web app)
// Execute as: Me. Who has access: Anyone.

const SHEET_NAME = "Leaderboard";
const SHARED_SECRET = "REPLACE_WITH_YOUR_OWN_SECRET_KEY";
const MAX_PLAUSIBLE_SCORE = 3000; // Adjust after real playtesting shows typical high scores

function doPost(e) {
  var sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName(SHEET_NAME);
  var params = e.parameter;

  if (params.secret !== SHARED_SECRET) {
    return jsonResponse({ success: false, error: "Invalid request" });
  }

  var name = (params.name || "").trim();
  var email = (params.email || "").trim();
  var phone = (params.phone || "").trim();

  if (!name || !email || !phone) {
    return jsonResponse({ success: false, error: "Missing required fields" });
  }

  var hasScore = params.score !== undefined && params.score !== "";
  var score = hasScore ? parseInt(params.score, 10) : 0;

  if (hasScore && (isNaN(score) || score < 0 || score > MAX_PLAUSIBLE_SCORE)) {
    return jsonResponse({ success: false, error: "Score rejected" });
  }

  var data = sheet.getDataRange().getValues();
  var rowIndex = -1;

  for (var i = 1; i < data.length; i++) {
    if (data[i][0] === name && data[i][1] === email && data[i][2] === phone) {
      rowIndex = i + 1;
      break;
    }
  }

  var now = new Date();

  if (rowIndex === -1) {
    var bestScore = hasScore ? score : 0;
    var lastScore = hasScore ? score : 0;
    var timesPlayed = hasScore ? 1 : 0;
    sheet.appendRow([name, email, phone, bestScore, lastScore, timesPlayed, now]);
    return jsonResponse({ success: true, isNewPlayer: true });
  }

  if (hasScore) {
    var currentBest = sheet.getRange(rowIndex, 4).getValue();
    var newBest = Math.max(currentBest, score);
    sheet.getRange(rowIndex, 4).setValue(newBest);
    sheet.getRange(rowIndex, 5).setValue(score);
    sheet.getRange(rowIndex, 6).setValue(sheet.getRange(rowIndex, 6).getValue() + 1);
    sheet.getRange(rowIndex, 7).setValue(now);
  }

  return jsonResponse({ success: true, isNewPlayer: false });
}

function jsonResponse(obj) {
  return ContentService.createTextOutput(JSON.stringify(obj))
    .setMimeType(ContentService.MimeType.JSON);
}
