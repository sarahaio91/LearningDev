<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="CAA | ABSTRACT | ESTREMI"/>


  <xsl:template match="/">
    <document>
      <xsl:apply-templates select="CIPER/LEGGI-SPEC"/>
    </document>
  </xsl:template>

  <xsl:template match="LEGGI-SPEC | COMUNE | COMMENTO">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="ARTICOLO">
    <xsl:apply-templates select="*[name() != 'RIF']"/>
    <xsl:apply-templates select="RIF"/>
  </xsl:template>

  <xsl:template match="DESCRIZIONE">
    <xsl:choose>
      <xsl:when test="parent::COMMENTO">
        <level>
          <div class="heading">
            <h6>
              <bold>
                <xsl:apply-templates select="PARA" mode="commento"/>
              </bold>
            </h6>
          </div>
          <xsl:apply-templates select="following-sibling::TESTO-COMM"/>
        </level>
      </xsl:when>
      <xsl:when test="parent::ARTICOLO">
        <div class="titel">
          <h3>
            <bold>
              <xsl:apply-templates select="PARA" mode="articolo"/>
            </bold>
          </h3>
        </div>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="sumText">
    <xsl:param name="sum"/>
    <xsl:param name="currentPosition"/>
    <xsl:variable name="newSum" select="string-length(translate(self::node()[position()],'&#x20;&#x9;&#xD;&#xA;','')) + $sum"/>
    <xsl:choose>
      <xsl:when test="following-sibling::node()">
        <xsl:call-template name="sumText">
          <xsl:with-param name="sum" select="$newSum"/>
          <xsl:with-param name="currentPosition" select="$currentPosition + 1"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$newSum"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="TESTO-COMM">
    <level>
      <xsl:for-each select="PARA/POSTILLA">
        <level>
          <xsl:variable name="prevText">
            <xsl:variable name="processNode" select="../preceding-sibling::PARA[1]/node()[1]"/>
              <xsl:choose>
                <xsl:when test="$processNode::*">
                </xsl:when>
                <xsl:when test="string-length(translate(self::node()[position()],'&#x20;&#x9;&#xD;&#xA;','')) = 0">
                </xsl:when>
                <xsl:otherwise>
                  <xsl:variable name="sumText">
                    <xsl:call-template name="sumText">
                      <xsl:with-param name="currentPosition" select="position()"/>
                      <xsl:with-param name="sum" select="0"/>
                    </xsl:call-template>
                  </xsl:variable>
                </xsl:otherwise>
              </xsl:choose>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="string-length(../preceding-sibling::PARA[1]/text()) &lt; 40">
              <italic>
                <xsl:value-of select="../preceding-sibling::PARA[1]/text()"/>
              </italic>
              <p>
                <xsl:apply-templates/>
              </p>
            </xsl:when>
            <xsl:otherwise>
              <p>
                <xsl:apply-templates/>
              </p>
            </xsl:otherwise>
          </xsl:choose>
        </level>
      </xsl:for-each>
    </level>
  </xsl:template>

  <xsl:template match="PARA" mode="articolo">
    <xsl:text>Art. </xsl:text>
    <xsl:value-of select="../../@testo"/>
    <xsl:text> </xsl:text>
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="PARA" mode="commento">
    <xsl:value-of select="../../@testo"/>
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="PARA">
    <xsl:apply-templates/>
    <br/>
  </xsl:template>

  <xsl:template match="TIPOG">
    <italic>
      <xsl:apply-templates/>
    </italic>
  </xsl:template>

  <xsl:template match="POSTILLA">
    <div class="POSTILLA">
      <xsl:apply-templates/>
    </div>
  </xsl:template>

  <xsl:template match="RIF">
    <div class="biblio">
      <xsl:apply-templates/>
    </div>
  </xsl:template>

  <xsl:template match="RICH-LEGGE-ART">

  </xsl:template>


  <xsl:template match="text()[not(normalize-space(.))]" />
</xsl:stylesheet>