<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

  <xsl:template match="@* | node()">
      <xsl:copy>
          <xsl:apply-templates select="@* | node()"/>
      </xsl:copy>
  </xsl:template>

  <!--<xsl:template match="div[@class='kopgeghftekst']">
    <xsl:apply-templates/>
  </xsl:template>-->

  <xsl:template match="div[@class='kopgeghftekst']">
    <div class="balk">
      <h2 class="kopgeghftekst" id="{a/span/@id}">
        <span class="hfteksttitel">
          <a class="hfteksttitel" href="{a/@href}">
            <xsl:apply-templates select="a/span/node()"/>
          </a>
        </span>
      </h2>
    </div>
  </xsl:template>
</xsl:stylesheet>
