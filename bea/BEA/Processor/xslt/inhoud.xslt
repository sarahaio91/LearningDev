<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" />

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:variable name="title" select="body/@title" />

  <xsl:template match="body">
    <body class="body1">
      <div class="inhoudsopgave">
        <div class="kopgeghftekst1" id="inhoudsopgave">Inhoudsopgave</div>
        <xsl:apply-templates select="div[@class='pe']"/>
        <p class="toc2">
          <br />
        </p>
        <p class="toc2">
          <a href="../Text/artikelen.html#toclink_paginaregister" id="backlink_paginaregister">Paginaregister</a>
        </p>
      </div>
    </body>
  </xsl:template>

  <xsl:template match="div[@class='pe']">
    <xsl:for-each select="p[@class = 'kopgeghftekst' or @class = 'ch-title' or @class = 'ch-title1']">
      <xsl:if test="@class='kopgeghftekst'">
        <p class="toc1">
          <a href="{concat('../Text/artikelen.html#', @id)}" id="{concat('back', @id)}">
            <xsl:apply-templates select="a/node()"/>
          </a>
        </p>
      </xsl:if>
      <xsl:if test="@class='ch-title'">
        <xsl:variable name="num" select="preceding-sibling::p[@class='ch-num'][1]/@id"/>
        <p class="toc2">
          <a href="{concat('../Text/artikelen.html#', @id)}" id="{concat('back', $num)}">
            <xsl:value-of select="preceding-sibling::*[name() = 'p'][position() = 1][@class = 'ch-num']/a/text()"/>
            <xsl:text>&#160;</xsl:text>
            <xsl:value-of select="."/>
          </a>
        </p>
      </xsl:if>
      <xsl:if test="@class='ch-title1' and not(text()='Fiscale signalementen')">
        <p class="toc2">
          <a href="{concat('../Text/artikelen.html#', @id)}" id="{concat('back', @id)}">
            <xsl:value-of select="following-sibling::*[name() = 'p'][@class = 'ch-num1']/a/text()"/>
            <xsl:text>&#160;</xsl:text>
            <xsl:value-of select="."/>
          </a>
        </p>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="text()[not(normalize-space(.))]" />
</xsl:stylesheet>

































<!--<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" />

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:variable name="title" select="body/@title" />

  <xsl:template match="body">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>
          <xsl:value-of select="$title" />
        </title>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link href="../Styles/stylesheet.css" type="text/css" rel="stylesheet" />
        <link href="../Styles/page_styles.css" type="text/css" rel="stylesheet" />
      </head>
      <body class="body1">
        <div class="inhoudsopgave">
          <div class="kopgeghftekst1" id="inhoudsopgave">Inhoudsopgave</div>
          <xsl:apply-templates select="p[@class='kopgeghftekst']"/>
          <xsl:apply-templates select="p[@class='ch-title']"/>
          <xsl:apply-templates select="p[@class='ch-title1']"/>
         </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="p[@class='kopgeghftekst']">
    <p class="toc1">
      <a>
        <xsl:apply-templates select="."/>
      </a>
    </p>
  </xsl:template>

  <xsl:template match="p[@class='ch-title']">
    <p class="toc2">
      <a>
        <xsl:value-of select="preceding-sibling::*[name() = 'p'][position() = 1][@class = 'ch-num']/a/text()"/>
        <xsl:text>&#160;</xsl:text>
        <xsl:value-of select="."/>
      </a>
    </p>
  </xsl:template>

  <xsl:template match="p[@class='ch-title1']">
    <p class="toc2">
      <a>
        <xsl:value-of select="following-sibling::*[name() = 'p'][@class = 'ch-num1']/a/text()"/>
        <xsl:text>&#160;</xsl:text>
        <xsl:value-of select="."/>
      </a>
    </p>
  </xsl:template>

  

  <xsl:template match="text()[not(normalize-space(.))]" />
</xsl:stylesheet>-->
