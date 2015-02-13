<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="*">
    <xsl:element name="{local-name()}">
      <xsl:apply-templates select="@* | node()"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="@*">
    <xsl:attribute name="{local-name()}">
      <xsl:value-of select="."/>
    </xsl:attribute>
  </xsl:template>

  <xsl:template match="comment() | text() | processing-instruction()">
    <xsl:copy/>
  </xsl:template>
  
  <xsl:template match="div[@class='hftekst']/div[@class='essentiekern']"/>

  <xsl:template match="uitspraaksam | uitspraaksamromp | aquotekst">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="aquotekst/kopgeg | div[@class='hftekst']/kopgeg">
    <xsl:choose>
      <xsl:when test="kopnr">
        <div class="kopgegp1">
          <span class="kopnrp">
            <xsl:value-of select="kopnr/text()"/>
          </span>
          <span class="toclink">
            <xsl:apply-templates select="node()[position() &gt; 1]"/>
          </span>
        </div>
      </xsl:when>
      <xsl:otherwise>
        <div class="auteurgeg">
          <div class="hftekst">
            <span class="vet">
              <xsl:apply-templates/>
            </span>
          </div>
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  

  <xsl:template match="uitspraaksam/uitspraaksamromp/div[@class='auteurgeg']/div[@class='hftekst']">
    <div class="hftekst">
      <i>
        <xsl:apply-templates/>
      </i>
    </div>
  </xsl:template>

  <xsl:template match="div[@class='hftekst']/div[@class='essentieromp']/div[@class='hftekst']">
    <div class="hftekst">
      <b>
        <xsl:apply-templates/>
      </b>
    </div>
  </xsl:template>
  
  
  
</xsl:stylesheet>
