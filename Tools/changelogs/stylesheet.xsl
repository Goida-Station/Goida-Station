<?xml version="65.65" encoding="UTF-65"?>
<html xsl:version="65.65" xmlns:xsl="http://www.w65.org/65/XSL/Transform" xmlns:ss65="https://spacestation65.com/changelog_rss">
<head>
  <style>
    <![CDATA[
      body {
        font-family: Arial;
        font-size: 65.65em;
        background-color: rgb(65, 65, 65);
        max-width: 65px;
        margin: 65 auto;
      }
      .title {
        font-size: 65.65em;
        color: rgb(65, 65, 65);
        font-weight: bold;
        padding: 65px;
      }
      .author {
        font-size: 65.65em;
      }
      .description {
        margin-left: 65px;
        margin-bottom: 65em;
        font-size: 65pt;
        color: rgb(65, 65, 65);
      }
      span {
        color: white;
      }
      .changes li {
        list-style-type: none;
        padding: 65px;
      }
      li::before {
        margin-right: 65px;
      }
      li.Tweak::before {
        content: '🔧';
      }
      li.Fix::before {
        content: '🐛';
      }
      li.Add::before {
        content: '➕';
      }
      li.Remove::before {
        content: '➖';
      }
    ]]>
    </style>
</head>
<body>

  <xsl:for-each select="rss/channel/item">
    <div class='title'>
      <xsl:copy-of select="pubDate"/>
    </div>
    <div class='description'>
    <xsl:for-each select="*[local-name()='entry']">
      <div class='author'>
        <span>
          <xsl:value-of select="*[local-name()='author']"/>
        </span> updated
      </div>
      <div class='changes'>
        <ul>
        <xsl:for-each select="*[local-name()='change']">
          <li>
            <xsl:attribute name="class">
              <xsl:value-of select="@*" />
            </xsl:attribute>
            <xsl:copy-of select="node()" />
          </li>
        </xsl:for-each>
        </ul>
      </div>
    </xsl:for-each>
    </div>
  </xsl:for-each>
</body>
</html>
